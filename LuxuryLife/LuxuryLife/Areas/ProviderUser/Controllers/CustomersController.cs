using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly TourBookingContext _context;

        public CustomersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Customers
        public async Task<IActionResult> Index(int id)
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;


            var customers = await _context.Bookings
                .Where(b => b.Tour != null && b.Tour.ProviderId == providerId)
                .Select(b => b.Customer)
                .Distinct()
                .ToListAsync();

            return View(customers);
        }

        // GET: ProviderUser/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", customer);
            }
            return View(customer);
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
