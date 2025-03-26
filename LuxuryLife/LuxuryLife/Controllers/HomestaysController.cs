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
    public class HomestaysController : Controller
    {
        private readonly TourBookingContext _context;

        public HomestaysController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Homestays
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.Homestays.Include(h => h.Tour).Include(h => h.Tour.Provider);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: Homestays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homestay = await _context.Homestays
                .Include(h => h.Tour)
                .FirstOrDefaultAsync(m => m.HomestayId == id);
            if (homestay == null)
            {
                return NotFound();
            }

            return View(homestay);
        }
        private bool HomestayExists(int id)
        {
            return _context.Homestays.Any(e => e.HomestayId == id);
        }
    }
}
