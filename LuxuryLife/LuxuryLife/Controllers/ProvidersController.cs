using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly TourBookingContext _context;

        public ProvidersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Providers.ToListAsync());
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Get CustomerId from session (nullable, no redirect if null)
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (id == null)
            {
                return NotFound();
            }

            // Fetch the provider asynchronously
            var provider = await _context.Providers
                .FirstOrDefaultAsync(m => m.ProviderId == id);
            if (provider == null)
            {
                return NotFound();
            }

            // Populate ViewBag with all necessary data
            ViewBag.Homestays = await _context.Homestays
                .Where(h => h.ProviderId == provider.ProviderId)
                .ToListAsync();
            ViewBag.Tours = await _context.Tours
                .Where(t => t.ProviderId == provider.ProviderId)
                .ToListAsync();
            ViewBag.Services = await _context.Services
                .Where(s => _context.Tours.Any(t => t.TourId == s.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();
            ViewBag.TourImages = await _context.Listimagestours
                .Where(li => _context.Tours.Any(t => t.TourId == li.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();
            ViewBag.Reviews = await _context.Reviews
                .Include(r => r.Customer) // Include customer data for reviews
                .Where(r => _context.Tours.Any(t => t.TourId == r.TourId && t.ProviderId == provider.ProviderId))
                .ToListAsync();
            ViewBag.ReviewOns = await _context.ReviewOns
                .Include(ro => ro.Customer) // Include customer data for replies
                .Where(ro => _context.Reviews.Any(r => r.ReviewId == ro.ReviewId &&
                    _context.Tours.Any(t => t.TourId == r.TourId && t.ProviderId == provider.ProviderId)))
                .ToListAsync();
            ViewBag.Customers = await _context.Customers.ToListAsync();

            // Set current user data in ViewBag only if logged in
            if (customerId.HasValue)
            {
                ViewBag.CurrentUser = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId.Value);
            }
            else
            {
                ViewBag.CurrentUser = null; // No current user if not logged in
            }

            return View(provider);
        }
        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.ProviderId == id);
        }
    }
}
