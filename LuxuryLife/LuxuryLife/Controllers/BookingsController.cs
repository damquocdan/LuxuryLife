using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using LuxuryLife.Controllers;
using Net.payOS.Types;
using Net.payOS;
using Microsoft.AspNetCore.Http;

namespace LuxuryLife.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TourBookingContext _context;
        private readonly IConfiguration _configuration;

        public BookingsController(TourBookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            var bookings = _context.Bookings.Include(b => b.Customer).Include(b => b.Tour).Where(b => b.CustomerId == customerId);
            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpGet]
        public IActionResult Create(int id, int customerId)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.TourId == id);
            if (tour == null) return NotFound("Tour không tồn tại");

            var booking = new Booking
            {
                TourId = id,
                CustomerId = customerId,
                CheckInDate = tour.StartDate,
                CheckOutDate = tour.EndDate,
                Status = "Pending",
                TotalPrice = tour.Price
            };

            ViewBag.TourName = tour.Name;
            ViewBag.CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId)?.Name;
            ViewBag.TotalPrice = tour.Price;
            ViewBag.CheckInDate = tour.StartDate;
            ViewBag.CheckOutDate = tour.EndDate;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            var existingBooking = _context.Bookings
                .FirstOrDefault(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);

            if (existingBooking != null)
            {
                ModelState.AddModelError("", "Bạn đã đặt tour này rồi, không thể đặt lại.");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                booking.BookingDate = DateTime.Now; // Gán ngày đặt tour
                booking.Status = "Pending";
                HttpContext.Session.SetString("PendingBooking", System.Text.Json.JsonSerializer.Serialize(booking));
                var paymentUrl = await CreatePayOSPayment(booking);
                if (!string.IsNullOrEmpty(paymentUrl))
                {
                    return Redirect(paymentUrl); // Chuyển hướng đến trang thanh toán PayOS
                }
                else
                {
                    ModelState.AddModelError("", "Không thể tạo liên kết thanh toán. Vui lòng thử lại.");
                    return View(booking);
                }
            }

            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null) _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        private async Task<string> CreatePayOSPayment(Booking booking)
        {
            var payOS = new PayOS(
                _configuration["PayOS:ClientId"],
                _configuration["PayOS:ApiKey"],
                _configuration["PayOS:ChecksumKey"]
            );

            long orderCode = DateTimeOffset.Now.ToUnixTimeSeconds(); // Tạo mã đơn hàng tạm thời từ timestamp
            int amount = Convert.ToInt32(booking.TotalPrice ?? 0);
            string description = $"Tour {booking.TourId} KH {booking.CustomerId}";
            string returnUrl = "https://localhost:7266/Bookings/PaymentSuccess";
            string cancelUrl = "https://localhost:7266/Bookings/PaymentCancelled";
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

            var response = await payOS.createPaymentLink(paymentData);
            HttpContext.Session.SetString("OrderCode", orderCode.ToString()); // Lưu orderCode dưới dạng chuỗi
            return response.checkoutUrl;
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            var orderCode = long.Parse(Request.Query["orderCode"]);
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0"); // Lấy và chuyển chuỗi thành long

            if (orderCode == storedOrderCode)
            {
                var bookingJson = HttpContext.Session.GetString("PendingBooking");
                if (!string.IsNullOrEmpty(bookingJson))
                {
                    var booking = System.Text.Json.JsonSerializer.Deserialize<Booking>(bookingJson);
                    booking.Status = "Confirmed";

                    _context.Add(booking);
                    await _context.SaveChangesAsync();

                    var favourite = _context.Favourites
                        .FirstOrDefault(f => f.TourId == booking.TourId && f.CustomerId == booking.CustomerId);
                    if (favourite != null)
                    {
                        _context.Favourites.Remove(favourite);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Message"] = "Thanh toán thành công! Tour của bạn đã được xác nhận.";
                    HttpContext.Session.Remove("PendingBooking");
                    HttpContext.Session.Remove("OrderCode");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PaymentCancelled()
        {
            var orderCode = long.Parse(Request.Query["orderCode"]);
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0"); // Lấy và chuyển chuỗi thành long

            if (orderCode == storedOrderCode)
            {
                var bookingJson = HttpContext.Session.GetString("PendingBooking");
                if (!string.IsNullOrEmpty(bookingJson))
                {
                    var booking = System.Text.Json.JsonSerializer.Deserialize<Booking>(bookingJson);
                    TempData["Message"] = "Thanh toán đã bị hủy. Vui lòng thử lại nếu cần.";

                    HttpContext.Session.Remove("PendingBooking");
                    HttpContext.Session.Remove("OrderCode");

                    var tour = _context.Tours.FirstOrDefault(t => t.TourId == booking.TourId);
                    ViewBag.TourName = tour?.Name;
                    ViewBag.CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == booking.CustomerId)?.Name;
                    ViewBag.TotalPrice = tour?.Price;
                    ViewBag.CheckInDate = tour?.StartDate;
                    ViewBag.CheckOutDate = tour?.EndDate;

                    return View("Create", booking);
                }
            }

            return RedirectToAction("Create");
        }
    }
}