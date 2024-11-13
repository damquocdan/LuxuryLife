using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.AdminQL.Controllers
{
    [Area("AdminQL")]
    public class FavoritesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public FavoritesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Favorites
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.Favorites.Include(f => f.Customer).Include(f => f.Homestay).Include(f => f.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: AdminQL/Favorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Customer)
                .Include(f => f.Homestay)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: AdminQL/Favorites/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: AdminQL/Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoriteId,CustomerId,HomestayId,TourId,CreateDate")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favorite.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", favorite.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // GET: AdminQL/Favorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favorite.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", favorite.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // POST: AdminQL/Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoriteId,CustomerId,HomestayId,TourId,CreateDate")] Favorite favorite)
        {
            if (id != favorite.FavoriteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.FavoriteId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favorite.CustomerId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", favorite.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // GET: AdminQL/Favorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
                .Include(f => f.Customer)
                .Include(f => f.Homestay)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: AdminQL/Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favorites.Any(e => e.FavoriteId == id);
        }
    }
}
