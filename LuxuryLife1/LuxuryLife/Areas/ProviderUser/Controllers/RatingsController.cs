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
    [Area("ProviderUser")]
    public class RatingsController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public RatingsController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Ratings
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.Ratings.Include(r => r.Customer).Include(r => r.Homestay).Include(r => r.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: ProviderUser/Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Homestay)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: ProviderUser/Ratings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: ProviderUser/Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingId,CustomerId,HomestayId,TourId,RatingValue,ReviewComment,CreateDate")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rating.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", rating.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", rating.TourId);
            return View(rating);
        }

        // GET: ProviderUser/Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rating.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", rating.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", rating.TourId);
            return View(rating);
        }

        // POST: ProviderUser/Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RatingId,CustomerId,HomestayId,TourId,RatingValue,ReviewComment,CreateDate")] Rating rating)
        {
            if (id != rating.RatingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.RatingId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", rating.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", rating.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", rating.TourId);
            return View(rating);
        }

        // GET: ProviderUser/Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Homestay)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: ProviderUser/Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Ratings.Any(e => e.RatingId == id);
        }
    }
}
