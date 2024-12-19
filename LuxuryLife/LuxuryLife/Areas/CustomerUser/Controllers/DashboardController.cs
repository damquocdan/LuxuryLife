using LuxuryLife.Controllers;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly TourBookingContext _context; // Thêm DbContext

        public DashboardController(ILogger<DashboardController> logger, TourBookingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Tours"] = _context.Tours
        .Include(t => t.Provider)
        .OrderByDescending(t => t.TourId) // Sắp xếp theo ngày tạo
        .Take(6) // Lấy 6 tour mới nhất
        .ToList();
            ViewData["Providers"] = _context.Providers.ToList();
            ViewData["News"] = _context.News.OrderByDescending(n => n.Createdate).Take(3).ToList();
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Reviews"] = _context.Reviews.Include(r => r.Tour).Include(r => r.Customer).ToList();
            return View();
        }
    }
}
