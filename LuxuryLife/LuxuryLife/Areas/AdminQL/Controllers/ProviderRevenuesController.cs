using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http;
using Net.payOS; // Thêm để dùng PayOS
using Net.payOS.Types; // Thêm để dùng PaymentData, ItemData

namespace LuxuryLife.Areas.AdminQL.Controllers
{
    [Area("AdminQL")]
    public class ProviderRevenuesController : Controller
    {
        private readonly TourBookingContext _context;
        private readonly IConfiguration _configuration; // Thêm để lấy cấu hình PayOS

        public ProviderRevenuesController(TourBookingContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: AdminQL/ProviderRevenues
        public async Task<IActionResult> Index(string searchProvider, int? monthFilter, int? yearFilter, string statusFilter, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["RevenueSortParm"] = sortOrder == "revenue" ? "revenue_desc" : "revenue";
            ViewData["CurrentProviderFilter"] = searchProvider;
            ViewData["CurrentYearFilter"] = yearFilter;

            var revenues = _context.ProviderRevenues.Include(p => p.Provider).AsQueryable();

            if (!string.IsNullOrEmpty(searchProvider))
            {
                revenues = revenues.Where(r => r.Provider.Name.Contains(searchProvider));
            }
            if (monthFilter.HasValue)
            {
                revenues = revenues.Where(r => r.RevenueMonth == monthFilter.Value);
            }
            if (yearFilter.HasValue)
            {
                revenues = revenues.Where(r => r.RevenueYear == yearFilter.Value);
            }
            if (!string.IsNullOrEmpty(statusFilter))
            {
                revenues = revenues.Where(r => r.Status == statusFilter);
            }

            switch (sortOrder)
            {
                case "revenue":
                    revenues = revenues.OrderBy(r => r.RevenueAmount);
                    break;
                case "revenue_desc":
                    revenues = revenues.OrderByDescending(r => r.RevenueAmount);
                    break;
                default:
                    revenues = revenues.OrderBy(r => r.RevenueId);
                    break;
            }

            return View(await revenues.ToListAsync());
        }

        // GET: AdminQL/ProviderRevenues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var providerRevenue = await _context.ProviderRevenues
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.RevenueId == id);
            if (providerRevenue == null) return NotFound();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", providerRevenue);
            }
            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email");
            return View();
        }

        // POST: AdminQL/ProviderRevenues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RevenueId,ProviderId,RevenueAmount,RevenueMonth,RevenueYear,CreatedAt,Status")] ProviderRevenue providerRevenue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(providerRevenue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var providerRevenue = await _context.ProviderRevenues.FindAsync(id);
            if (providerRevenue == null) return NotFound();

            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // POST: AdminQL/ProviderRevenues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RevenueId,ProviderId,RevenueAmount,RevenueMonth,RevenueYear,CreatedAt,Status")] ProviderRevenue providerRevenue)
        {
            if (id != providerRevenue.RevenueId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(providerRevenue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderRevenueExists(providerRevenue.RevenueId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var providerRevenue = await _context.ProviderRevenues
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.RevenueId == id);
            if (providerRevenue == null) return NotFound();

            return View(providerRevenue);
        }

        // POST: AdminQL/ProviderRevenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var providerRevenue = await _context.ProviderRevenues.FindAsync(id);
            if (providerRevenue != null)
            {
                _context.ProviderRevenues.Remove(providerRevenue);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderRevenueExists(int id)
        {
            return _context.ProviderRevenues.Any(e => e.RevenueId == id);
        }

        // Generate QR for payment with PayOS integration
        public async Task<IActionResult> GeneratePaymentQR(int? id)
        {
            if (id == null) return NotFound();

            var providerRevenue = await _context.ProviderRevenues
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.RevenueId == id);

            if (providerRevenue == null) return NotFound();

            // Tạo URL thanh toán với PayOS
            string paymentUrl = await CreatePayOSPayment(providerRevenue);

            // Tạo mã QR từ URL thanh toán
            string qrFilePath = await GenerateQRCode(paymentUrl, providerRevenue.RevenueId.ToString());

            // Lưu thông tin vào Session và ViewBag
            HttpContext.Session.SetString("PendingRevenueId", providerRevenue.RevenueId.ToString());
            ViewBag.QRFilePath = qrFilePath;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PaymentDetails", providerRevenue);
            }
            return View(providerRevenue);
        }

        // Generate QR Code using external API
        private async Task<string> GenerateQRCode(string content, string fileName)
        {
            using (var httpClient = new HttpClient())
            {
                string qrApiUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={Uri.EscapeDataString(content)}";
                var response = await httpClient.GetAsync(qrApiUrl);
                response.EnsureSuccessStatusCode();

                string qrFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes");
                if (!Directory.Exists(qrFolder))
                {
                    Directory.CreateDirectory(qrFolder);
                }

                string qrFilePath = Path.Combine(qrFolder, $"{fileName}.png");
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(qrFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return $"/qrcodes/{fileName}.png";
            }
        }

        // Create PayOS payment link
        private async Task<string> CreatePayOSPayment(ProviderRevenue revenue)
        {
            var payOS = new PayOS(
                _configuration["PayOS:ClientId"],
                _configuration["PayOS:ApiKey"],
                _configuration["PayOS:ChecksumKey"]
            );

            long orderCode = DateTimeOffset.Now.ToUnixTimeSeconds();
            int amount = Convert.ToInt32(revenue.RevenueAmount);
            string description = $"Thanh toán doanh thu {revenue.RevenueId}";
            string returnUrl = "https://localhost:7266/AdminQL/ProviderRevenues/PaymentSuccess";
            string cancelUrl = "https://localhost:7266/AdminQL/ProviderRevenues/Index";
            long expirationTime = ((DateTimeOffset)DateTime.UtcNow.AddMinutes(30)).ToUnixTimeSeconds();

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: amount,
                description: description.Length > 25 ? description.Substring(0, 25) : description,
                items: [new ItemData($"Revenue {revenue.RevenueId}", 1, amount)],
                returnUrl: returnUrl,
                cancelUrl: cancelUrl,
                expiredAt: expirationTime
            );

            var response = await payOS.createPaymentLink(paymentData);
            HttpContext.Session.SetString("OrderCode", orderCode.ToString());
            return response.checkoutUrl;
        }

        // Handle payment success
        public async Task<IActionResult> PaymentSuccess()
        {
            var orderCode = long.Parse(Request.Query["orderCode"].ToString());
            var storedOrderCode = long.Parse(HttpContext.Session.GetString("OrderCode") ?? "0");

            if (orderCode == storedOrderCode)
            {
                var revenueId = int.Parse(HttpContext.Session.GetString("PendingRevenueId") ?? "0");
                var providerRevenue = await _context.ProviderRevenues.FindAsync(revenueId);
                if (providerRevenue != null)
                {
                    providerRevenue.Status = "Confirmed";
                    _context.Update(providerRevenue);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove("PendingRevenueId");
                    HttpContext.Session.Remove("OrderCode");

                    TempData["Message"] = "Thanh toán thành công!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var providerRevenue = await _context.ProviderRevenues.FindAsync(id);
            if (providerRevenue == null)
            {
                return Json(new { success = false, message = "Không tìm thấy doanh thu." });
            }

            if (providerRevenue.Status == "Confirmed")
            {
                return Json(new { success = false, message = "Doanh thu này đã được thanh toán." });
            }

            providerRevenue.Status = "Confirmed";
            _context.Update(providerRevenue);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thanh toán thành công!" });
        }
    }
}