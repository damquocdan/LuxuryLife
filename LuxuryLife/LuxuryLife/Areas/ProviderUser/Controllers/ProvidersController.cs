using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    [Area("ProviderUser")]
    public class ProvidersController : Controller
    {
        private readonly TourBookingContext _context;

        public ProvidersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(p => p.ProviderId == id);

            if (provider == null)
            {
                return NotFound();
            }

            // Fetch latest tours with images
            var tours = await _context.Tours
                .Include(t => t.Listimagestours)
                .Where(t => t.ProviderId == id && t.Status == "Active")
                .OrderByDescending(t => t.Createdate)
                .Take(3)
                .ToListAsync();

            // Fetch latest services
            var services = await _context.Services
                .Include(s => s.Tour)
                .Where(s => s.Tour.ProviderId == id)
                .OrderByDescending(s => s.Tour.Createdate)
                .Take(3)
                .ToListAsync();

            // Fetch booked customers
            var bookedCustomers = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .Where(b => b.Tour.ProviderId == id)
                .Select(b => new
                {
                    CustomerName = b.Customer.Name,
                    CustomerEmail = b.Customer.Email,
                    CustomerAvatar = b.Customer.Avatar ?? "/images/default-avatar.png", // Default avatar if null
                    TourName = b.Tour.Name,
                    BookingDate = b.BookingDate,
                    TotalPrice = b.TotalPrice
                })
                .OrderByDescending(b => b.BookingDate)
                .Take(5) // Limit to 5 customers
                .ToListAsync();

            ViewBag.Tours = tours;
            ViewBag.Services = services;
            ViewBag.BookedCustomers = bookedCustomers;

            return View(provider);
        }
    }
}