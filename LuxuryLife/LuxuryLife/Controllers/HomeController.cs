
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LuxuryLife.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TourBookingContext _context; // Thêm DbContext

        public HomeController(ILogger<HomeController> logger, TourBookingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Tours"] = _context.Tours
                .Where(t => t.Status == "Active") // Filter by active status
                .Include(t => t.Provider)
                .OrderByDescending(t => t.TourId) // Sort by TourId in descending order (newest first)
                .Take(6) // Get only the 6 latest active tours
                .ToList();

            ViewData["Providers"] = _context.Providers.ToList();
            ViewData["News"] = _context.News.OrderByDescending(n => n.Createdate).Take(3).ToList();
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Reviews"] = _context.Reviews.Include(r => r.Tour).Include(r => r.Customer).ToList();

            return View();
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateEmailProvider([Bind("ProviderId,Name,Email,Password,Avatar,Phone,Address,Rating,Createdate")] Provider provider)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(provider);
        //        await _context.SaveChangesAsync();

        //        // Lưu thông báo thành công vào TempData
        //        TempData["SuccessMessage"] = "Email của bạn đã được gửi thành công!";

        //        // Redirect lại chính trang này sau khi thành công
        //        return RedirectToAction(nameof(Index));  // Thay đổi RedirectToAction thành chính action này
        //    }

        //    // Nếu có lỗi, trả lại lại view với dữ liệu ban đầu
        //    return View(provider);
        //}



        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
