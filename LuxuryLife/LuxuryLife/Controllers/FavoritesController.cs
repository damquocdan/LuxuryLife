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
    public class FavoritesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public FavoritesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.Favorites.Include(f => f.Customer).Include(f => f.Homestay).Include(f => f.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }


        // ADD
        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            // Lấy CustomerId từ session
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Kiểm tra nếu CustomerId không tồn tại trong session
            if (customerId == null)
            {
                TempData["Error"] = "Bạn cần đăng nhập để sử dụng chức năng này.";
                return RedirectToAction("Index", "Login", new { area = "CustomerUser" });

            }

            // Kiểm tra nếu tour đã tồn tại trong danh sách yêu thích
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.TourId == id && f.CustomerId == customerId);

            if (existingFavorite != null)
            {
                TempData["Error"] = "Tour đã có trong danh sách yêu thích!";
                return RedirectToAction("Index", "Home");
            }

            // Thêm tour vào danh sách yêu thích
            var favorite = new Favorite
            {
                CustomerId = customerId.Value, // CustomerId được lấy từ session
                TourId = id,
                CreateDate = DateTime.Now
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tour đã được thêm vào danh sách yêu thích!";
            return RedirectToAction("Index", "Home");
        }
        // GET: Favorites/Details/5
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

        // GET: Favorites/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: Favorites/Create
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

        // GET: Favorites/Edit/5
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

        // POST: Favorites/Edit/5
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

        // GET: Favorites/Delete/5
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

        // POST: Favorites/Delete/5
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
