using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class ToursController : Controller
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: CustomerUser/Tours
        public async Task<IActionResult> Index(string query, string description)
        {
            // Initialize tours queryable
            var tours = _context.Tours
                .Where(t => t.Status == "Active") // Filter by active status
                .Include(t => t.Provider) // Include related data
                .AsQueryable();

            // Filter by query (name or description)
            if (!string.IsNullOrEmpty(query))
            {
                tours = tours.Where(t => t.Name.Contains(query) || t.Description.Contains(query));
            }

            // Additional filter by description
            if (!string.IsNullOrEmpty(description))
            {
                tours = tours.Where(t => t.Description.Contains(description));
            }

            // Fetch filtered tours asynchronously
            var filteredTours = await tours
                .OrderByDescending(t => t.TourId) // Sort by TourId in descending order
                .ToListAsync();

            // Load other necessary data
            ViewData["Tours"] = filteredTours;
            ViewData["Providers"] = await _context.Providers.ToListAsync();
            ViewData["Customers"] = await _context.Customers.ToListAsync();
            ViewData["Reviews"] = await _context.Reviews
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .ToListAsync();

            // Pass the filtered tours to the view
            return View(filteredTours);
        }


        // GET: CustomerUser/Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Provider)
                .Include(l => l.Listimagestours)
                .Include(t => t.Services)
                .Include(c => c.Homestays)
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // GET: CustomerUser/Tours/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email");
            return View();
        }

        // POST: CustomerUser/Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Price,Status,Createdate,ProviderId")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", tour.ProviderId);
            return View(tour);
        }

        // GET: CustomerUser/Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", tour.ProviderId);
            return View(tour);
        }

        // POST: CustomerUser/Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Price,Status,Createdate,ProviderId")] Tour tour)
        {
            if (id != tour.TourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.TourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", tour.ProviderId);
            return View(tour);
        }

        // GET: CustomerUser/Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Provider)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        // POST: CustomerUser/Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}
