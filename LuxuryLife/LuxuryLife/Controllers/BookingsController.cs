using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using Net.payOS;
using System.Text.Json;
using System.IO;
using System.Net.Http;

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
        public IActionResult Create(int? id, int? customerId)
        {
            if (!id.HasValue || !customerId.HasValue)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "Login");
            }

            var tour = _context.Tours.FirstOrDefault(t => t.TourId == id.Value);
            if (tour == null)
            {
                return NotFound("Tour không tồn tại.");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId.Value);
            if (customer == null)
            {
                return NotFound("Khách hàng không tồn tại.");
            }

            var booking = new Booking
            {
                TourId = id.Value,
                CustomerId = customerId.Value
            };

            ViewBag.TourName = tour.Name;
            ViewBag.CustomerName = customer.Name;
            ViewBag.PriceTour = tour.Price;
            ViewBag.PricePerson = tour.PricePerson;
            ViewBag.StartDate = tour.StartDate;
            ViewBag.EndDate = tour.EndDate;
            ViewBag.Day = tour.Day;
            ViewBag.Image = tour.Image;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,TourId,BookingDate,NumberOfGuests,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            if (booking == null)
            {
                return BadRequest("Thông tin đặt tour không hợp lệ.");
            }

            var tour = await _context.Tours.FindAsync(booking.TourId);
            if (tour == null)
            {
                ModelState.AddModelError("", "Tour không tồn tại.");
                return View(booking);
            }

            var customer = await _context.Customers.FindAsync(booking.CustomerId);
            if (customer == null)
            {
                ModelState.AddModelError("", "Khách hàng không tồn tại.");
                return View(booking);
            }

            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);

            if (existingBooking != null)
            {
                ModelState.AddModelError("", "Bạn đã đặt tour này rồi, không thể đặt lại.");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set booking details (chưa lưu vào database)
                    booking.BookingDate = DateTime.UtcNow;
                    booking.Status = "Pending";

                    // Lưu thông tin đặt tour tạm thời vào session
                    HttpContext.Session.SetString("PendingBooking", JsonSerializer.Serialize(booking));

                    // Tạo liên kết thanh toán
                    var paymentUrl = await CreatePayOSPayment(booking);
                    if (!string.IsNullOrEmpty(paymentUrl))
                    {
                        // Không lưu booking vào database ở đây, chờ PaymentSuccess
                        return Redirect(paymentUrl);
                    }

                    ModelState.AddModelError("", "Không thể tạo liên kết thanh toán. Vui lòng thử lại.");
                    return View(booking);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                    return View(booking);
                }
            }

            // Populate ViewBag for the view
            ViewBag.TourName = tour.Name;
            ViewBag.CustomerName = customer.Name;
            ViewBag.PriceTour = tour.Price;
            ViewBag.PricePerson = tour.PricePerson;
            ViewBag.StartDate = tour.StartDate;
            ViewBag.EndDate = tour.EndDate;
            ViewBag.Day = tour.Day;
            ViewBag.Image = tour.Image;

            return View(booking);
        }

        private async Task<string> GenerateQRCode(string content, string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                string qrApiUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={Uri.EscapeDataString(content)}";
                var response = await httpClient.GetAsync(qrApiUrl);
                response.EnsureSuccessStatusCode();

                string qrFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "bookings");
                if (!Directory.Exists(qrFolder))
                {
                    Directory.CreateDirectory(qrFolder);
                }

                string qrFilePath = Path.Combine(qrFolder, $"code_{fileName}.png");
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(qrFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return $"/images/bookings/code_{fileName}.png";
            }
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

            long orderCode = DateTimeOffset.Now.ToUnixTimeSeconds();
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
            HttpContext.Session.SetString("OrderCode", orderCode.ToString());
            return response.checkoutUrl;
        }

        public async Task<IActionResult> PaymentSuccess()
        {
            var orderCode = long.Parse(Request.Query["orderCode"].ToString());
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0");

            if (orderCode == storedOrderCode)
            {
                var bookingJson = HttpContext.Session.GetString("PendingBooking");
                if (!string.IsNullOrEmpty(bookingJson))
                {
                    var booking = JsonSerializer.Deserialize<Booking>(bookingJson);
                    booking.Status = "Confirmed"; // Đặt trạng thái là Confirmed
                    booking.BookingDate = DateTime.UtcNow; // Cập nhật lại ngày đặt tại thời điểm xác nhận

                    // Kiểm tra lại để chắc chắn không có booking trùng
                    var existingBooking = await _context.Bookings
                        .FirstOrDefaultAsync(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);
                    if (existingBooking != null)
                    {
                        TempData["Message"] = "Booking này đã tồn tại.";
                        HttpContext.Session.Remove("PendingBooking");
                        HttpContext.Session.Remove("OrderCode");
                        return RedirectToAction(nameof(Index));
                    }

                    // Thêm booking vào DB
                    _context.Bookings.Add(booking);
                    await _context.SaveChangesAsync();

                    // Lấy thông tin tour và customer
                    var tour = await _context.Tours.FindAsync(booking.TourId);
                    var customer = await _context.Customers.FindAsync(booking.CustomerId);

                    // Tạo nội dung cho mã QR với CheckInDate và CheckOutDate
                    string qrContent = $"Mã đơn đặt: {booking.BookingId}\n" +
                                      $"Tên người đặt: {customer.Name}\n" +
                                      $"Tour du lịch: {tour.Name}\n" +
                                      $"Số người: {booking.NumberOfGuests}\n" +
                                      $"Tổng giá: {booking.TotalPrice:N0} VND\n" +
                                      $"Ngày bắt đầu: {booking.CheckInDate:yyyy-MM-dd}\n" +
                                      $"Ngày kết thúc: {booking.CheckOutDate:yyyy-MM-dd}";

                    // Tạo mã QR và lưu
                    string qrFilePath = await GenerateQRCode(qrContent, booking.BookingId.ToString());
                    booking.Code = qrFilePath;
                    await _context.SaveChangesAsync();

                    // Xóa favourite nếu có
                    var favourite = _context.Favourites
                        .FirstOrDefault(f => f.TourId == booking.TourId && f.CustomerId == booking.CustomerId);
                    if (favourite != null)
                    {
                        _context.Favourites.Remove(favourite);
                        await _context.SaveChangesAsync();
                    }

                    // Cập nhật doanh thu cho nhà cung cấp
                    await _context.Database.ExecuteSqlRawAsync("EXEC UpdateProviderRevenue");

                    TempData["Message"] = "Thanh toán thành công! Tour của bạn đã được xác nhận.";
                    HttpContext.Session.Remove("PendingBooking");
                    HttpContext.Session.Remove("OrderCode");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PaymentCancelled()
        {
            var orderCode = long.Parse(Request.Query["orderCode"].ToString());
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0");

            if (orderCode == storedOrderCode)
            {
                var bookingJson = HttpContext.Session.GetString("PendingBooking");
                if (!string.IsNullOrEmpty(bookingJson))
                {
                    var booking = JsonSerializer.Deserialize<Booking>(bookingJson);
                    TempData["Message"] = "Thanh toán đã bị hủy. Vui lòng thử lại nếu cần.";

                    HttpContext.Session.Remove("PendingBooking");
                    HttpContext.Session.Remove("OrderCode");

                    var tour = _context.Tours.FirstOrDefault(t => t.TourId == booking.TourId);
                    ViewBag.TourName = tour?.Name;
                    ViewBag.CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == booking.CustomerId)?.Name;
                    ViewBag.PriceTour = tour?.Price;
                    ViewBag.PricePerson = tour?.PricePerson;
                    ViewBag.StartDate = tour?.StartDate;
                    ViewBag.EndDate = tour?.EndDate;
                    ViewBag.Day = tour?.Day;
                    ViewBag.Image = tour?.Image;
                    return View("Create", booking);
                }
            }

            return RedirectToAction("Create");
        }
    }
}