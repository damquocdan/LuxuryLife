using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly TourBookingContext _context;
        private readonly IConfiguration _configuration;
        private readonly PayOS _payOS;

        public BookingsController(TourBookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _payOS = new PayOS(
                _configuration["PayOS:ClientId"],
                _configuration["PayOS:ApiKey"],
                _configuration["PayOS:ChecksumKey"]
            );
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return Unauthorized(new { message = "You must be logged in to view bookings." });
            }

            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // POST: api/Bookings/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest(new { message = "Invalid booking information." });
            }

            var tour = await _context.Tours.FindAsync(booking.TourId);
            if (tour == null)
            {
                return NotFound(new { message = "Tour does not exist." });
            }

            var customer = await _context.Customers.FindAsync(booking.CustomerId);
            if (customer == null)
            {
                return NotFound(new { message = "Customer does not exist." });
            }

            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);

            if (existingBooking != null)
            {
                return BadRequest(new { message = "You have already booked this tour." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                booking.BookingDate = DateTime.UtcNow;
                booking.Status = "Pending";

                HttpContext.Session.SetString("PendingBooking", JsonSerializer.Serialize(booking));
                var paymentUrl = await CreatePayOSPayment(booking);

                if (!string.IsNullOrEmpty(paymentUrl))
                {
                    return Ok(new { paymentUrl });
                }

                return BadRequest(new { message = "Failed to create payment link. Please try again." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest(new { message = "Booking ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(booking).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Bookings/PaymentSuccess
        [HttpGet("PaymentSuccess")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] long orderCode)
        {
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0");

            if (orderCode != storedOrderCode)
            {
                return BadRequest(new { message = "Invalid order code." });
            }

            var bookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(bookingJson))
            {
                return BadRequest(new { message = "No pending booking found." });
            }

            var booking = JsonSerializer.Deserialize<Booking>(bookingJson);
            booking.Status = "Confirmed";
            booking.BookingDate = DateTime.UtcNow;

            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);
            if (existingBooking != null)
            {
                HttpContext.Session.Remove("PendingBooking");
                HttpContext.Session.Remove("OrderCode");
                return BadRequest(new { message = "This booking already exists." });
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var tour = await _context.Tours.FindAsync(booking.TourId);
            var customer = await _context.Customers.FindAsync(booking.CustomerId);

            string qrContent = $"Booking ID: {booking.BookingId}\n" +
                              $"Customer Name: {customer.Name}\n" +
                              $"Tour: {tour.Name}\n" +
                              $"Guests: {booking.NumberOfGuests}\n" +
                              $"Total Price: {booking.TotalPrice:N0} VND\n" +
                              $"Check-In: {booking.CheckInDate:yyyy-MM-dd}\n" +
                              $"Check-Out: {booking.CheckOutDate:yyyy-MM-dd}";

            string qrFilePath = await GenerateQRCode(qrContent, booking.BookingId.ToString());
            booking.Code = qrFilePath;
            await _context.SaveChangesAsync();

            var favourite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.TourId == booking.TourId && f.CustomerId == booking.CustomerId);
            if (favourite != null)
            {
                _context.Favourites.Remove(favourite);
                await _context.SaveChangesAsync();
            }

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateProviderRevenue");

            HttpContext.Session.Remove("PendingBooking");
            HttpContext.Session.Remove("OrderCode");

            return Ok(new { message = "Payment successful! Your tour has been confirmed.", booking });
        }

        // GET: api/Bookings/PaymentCancelled
        [HttpGet("PaymentCancelled")]
        public IActionResult PaymentCancelled([FromQuery] long orderCode)
        {
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0");

            if (orderCode != storedOrderCode)
            {
                return BadRequest(new { message = "Invalid order code." });
            }

            var bookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(bookingJson))
            {
                return BadRequest(new { message = "No pending booking found." });
            }

            var booking = JsonSerializer.Deserialize<Booking>(bookingJson);
            HttpContext.Session.Remove("PendingBooking");
            HttpContext.Session.Remove("OrderCode");

            return Ok(new { message = "Payment cancelled. Please try again if needed.", booking });
        }

        private async Task<string> CreatePayOSPayment(Booking booking)
        {
            long orderCode = DateTimeOffset.Now.ToUnixTimeSeconds();
            int amount = Convert.ToInt32(booking.TotalPrice ?? 0);
            string description = $"Tour {booking.TourId} KH {booking.CustomerId}";
            string returnUrl = "https://localhost:7266/api/Bookings/PaymentSuccess";
            string cancelUrl = "https://localhost:7266/api/Bookings/PaymentCancelled";
            long expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(30)).ToUnixTimeSeconds();

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: amount,
                description: description.Length > 25 ? description.Substring(0, 25) : description,
                items: [new ItemData($"Tour {booking.TourId}", 1, amount)],
                returnUrl: returnUrl,
                cancelUrl: cancelUrl,
                expiredAt: expirationTime
            );

            var response = await _payOS.createPaymentLink(paymentData);
            HttpContext.Session.SetString("OrderCode", orderCode.ToString());
            return response.checkoutUrl;
        }

        private async Task<string> GenerateQRCode(string content, string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                string qrApiUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={Uri.EscapeDataString(content)}";
                var response = await httpClient.GetAsync(qrApiUrl);
                response.EnsureSuccessStatusCode();

                string qrFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "bookings");
                Directory.CreateDirectory(qrFolder);

                string qrFilePath = Path.Combine(qrFolder, $"code_{fileName}.png");
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(qrFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return $"/images/bookings/code_{fileName}.png";
            }
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}