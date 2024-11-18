using LuxuryLife.Data;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LuxuryLife.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LuxuryLifeContext _context; // Thêm DbContext

        public HomeController(ILogger<HomeController> logger, LuxuryLifeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var tours = _context.Tours.Include(t => t.Provider).ToList();
            return View(tours);
        }

        public IActionResult Privacy()
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
