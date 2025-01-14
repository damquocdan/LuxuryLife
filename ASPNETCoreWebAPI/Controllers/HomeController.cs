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

        [HttpGet("Index")]
        public IActionResult GetIndexData()
        {
            var data = new
            {
                Tours = _context.Tours
                    .Where(t => t.Status == "Active")
                    .Include(t => t.Provider)
                    .OrderByDescending(t => t.TourId)
                    .Take(6)
                    .Select(t => new
                    {
                        t.TourId,
                        t.Name,
                        t.Description,
                        t.Image,
                        t.Price,
                        ProviderName = t.Provider.Name,
                        ProviderAvatar = t.Provider.Avatar,
                        
                    })
                    .ToList(),

                Providers = _context.Providers
                    .Select(p => new
                    {
                        p.ProviderId,
                        p.Name,
                        p.Address
                    })
                    .ToList(),

                News = _context.News
                    .OrderByDescending(n => n.Createdate)
                    .Take(3)
                    .Select(n => new
                    {
                        n.NewId,
                        n.Title,
                        n.Content,
                        n.Createdate
                    })
                    .ToList(),

                Customers = _context.Customers
                    .Select(c => new
                    {
                        c.CustomerId,
                        c.Name,
                        c.Email
                    })
                    .ToList(),

                Reviews = _context.Reviews
                    .Include(r => r.Tour)
                    .Include(r => r.Customer)
                    .Select(r => new
                    {
                        r.ReviewId,
                        r.Comment,
                        TourTitle = r.Tour.Name,
                        CustomerName = r.Customer.Name
                    })
                    .ToList()
            };

            return Ok(data);
        }
    }
}
