using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ServicesController : Controller
    {
        private readonly TourBookingContext _context;

        public ServicesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Services?category=Nhà hàng
        public async Task<IActionResult> Index(string category = null)
        {
            var servicesQuery = _context.Services.Include(s => s.Tour).Include(s => s.Tour.Provider).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                category = category.ToLower(); // Normalize the input
                servicesQuery = servicesQuery.Where(s =>
                    s.ServiceName.ToLower().Contains(category) ||
                    (s.Description != null && s.Description.ToLower().Contains(category)));
            }

            var services = await servicesQuery.ToListAsync();
            ViewBag.SelectedCategory = category;
            return View(services);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}