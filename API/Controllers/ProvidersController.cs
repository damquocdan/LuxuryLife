using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ProvidersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Provider>>> GetProviders()
        {
            var providers = await _context.Providers.ToListAsync();
            return Ok(providers);
        }

        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProvider(int id)
        {
            // Get CustomerId from session (nullable, no redirect if null)
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Fetch the provider asynchronously
            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.ProviderId == id);

            if (provider == null)
            {
                return NotFound();
            }

            // Fetch related data
            var homestays = await _context.Homestays
                .Where(h => h.ProviderId == provider.ProviderId)
                .ToListAsync();

            var tours = await _context.Tours
                .Where(t => t.ProviderId == provider.ProviderId)
                .ToListAsync();

            var services = await _context.Services
                .Where(s => _context.Tours.Any(t => t.TourId == s.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();

            var tourImages = await _context.Listimagestours
                .Where(li => _context.Tours.Any(t => t.TourId == li.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();

            var reviews = await _context.Reviews
                .Include(r => r.Customer) // Include customer data for reviews
                .Where(r => _context.Tours.Any(t => t.TourId == r.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();

            var reviewOns = await _context.ReviewOns
                .Include(ro => ro.Customer) // Include customer data for replies
                .Where(ro => _context.Reviews.Any(r => r.ReviewId == ro.ReviewId &&
                    _context.Tours.Any(t => t.TourId == r.TourId && t.ProviderId == provider.ProviderId)))
                .ToListAsync();

            var customers = await _context.Customers.ToListAsync();

            // Set current user data only if logged in
            var currentUser = customerId.HasValue
                ? await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId.Value)
                : null;

            // Construct the response object
            var response = new
            {
                Provider = provider,
                Homestays = homestays,
                Tours = tours,
                Services = services,
                TourImages = tourImages,
                Reviews = reviews,
                ReviewOns = reviewOns,
                Customers = customers,
                CurrentUser = currentUser
            };

            return Ok(response);
        }

        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.ProviderId == id);
        }
    }
}