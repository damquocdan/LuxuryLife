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
    public class FavouritesController : Controller
    {
        private readonly TourBookingContext _context;

        public FavouritesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Favourites
        public async Task<IActionResult> Index()
        {
            var tourbookingContext = _context.Favourites.Include(f => f.Customer).Include(f => f.Tour);
            return View(await tourbookingContext.ToListAsync());
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
            var existingFavorite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.TourId == id && f.CustomerId == customerId);

            if (existingFavorite != null)
            {
                TempData["Error"] = "Tour đã có trong danh sách yêu thích!";
                return RedirectToAction("Index", "Home");
            }

            // Thêm tour vào danh sách yêu thích
            var favorite = new Favourite
            {
                CustomerId = customerId.Value, // CustomerId được lấy từ session
                TourId = id,
            };

            _context.Favourites.Add(favorite);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tour đã được thêm vào danh sách yêu thích!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Favourites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favourite = await _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favourite == null)
            {
                return NotFound();
            }

            return View(favourite);
        }

        // GET: Favourites/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: Favourites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoriteId,CustomerId,TourId,Sumprice")] Favourite favourite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favourite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favourite.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favourite.TourId);
            return View(favourite);
        }

        // GET: Favourites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favourite.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favourite.TourId);
            return View(favourite);
        }

        // POST: Favourites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoriteId,CustomerId,TourId,Sumprice")] Favourite favourite)
        {
            if (id != favourite.FavoriteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favourite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavouriteExists(favourite.FavoriteId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favourite.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favourite.TourId);
            return View(favourite);
        }

        // GET: Favourites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favourite = await _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favourite == null)
            {
                return NotFound();
            }

            return View(favourite);
        }

        // POST: Favourites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite != null)
            {
                _context.Favourites.Remove(favourite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavouriteExists(int id)
        {
            return _context.Favourites.Any(e => e.FavoriteId == id);
        }
    }
}
