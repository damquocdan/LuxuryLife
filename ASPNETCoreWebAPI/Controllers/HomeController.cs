using ASPNETCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TourBookingContext _context;

        public HomeController(ILogger<HomeController> logger, TourBookingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("latest-tours")]
        public IActionResult GetLatestTours()
        {
            var tours = _context.Tours
                .Where(t => t.Status == "Active")
                .Include(t => t.Provider)
                .OrderByDescending(t => t.TourId)
                .Take(6)
                .ToList();

            return Ok(tours);
        }
        [HttpGet("providers")]
        public IActionResult GetProviders()
        {
            var providers = _context.Providers.ToList();
            return Ok(providers);
        }

        [HttpGet("latest-news")]
        public IActionResult GetLatestNews()
        {
            var news = _context.News
                .OrderByDescending(n => n.Createdate)
                .Take(3)
                .ToList();

            return Ok(news);
        }

        [HttpGet("customers")]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        [HttpGet("reviews")]
        public IActionResult GetReviews()
        {
            var reviews = _context.Reviews
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .ToList();

            return Ok(reviews);
        }
    }
}
