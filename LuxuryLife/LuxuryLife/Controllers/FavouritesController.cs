using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Authorization;

namespace LuxuryLife.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly TourBookingContext _context;

        public FavouritesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: CustomerUser/Favorites
        public async Task<IActionResult> Index()
        {
            // Lấy CustomerId từ session
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Kiểm tra nếu CustomerId không tồn tại (người dùng chưa đăng nhập)
            if (customerId == null)
            {
                // Chuyển hướng đến trang đăng nhập
                return RedirectToAction("Index", "Login", new { area = "CustomerUser" });
            }

            // Lọc danh sách yêu thích theo CustomerId
            var luxuryLifeContext = _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .Where(f => f.CustomerId == customerId); // Lọc theo CustomerId

            // Trả danh sách yêu thích của người dùng đến view
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
            var existingFavorite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.TourId == id && f.CustomerId == customerId);

            if (existingFavorite != null)
            {
                TempData["Error"] = "Tour đã có trong danh sách yêu thích!";
                return RedirectToAction("Index", "Dashboard");
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
            return RedirectToAction("Index", "Dashboard");
        }
        // GET: CustomerUser/Favorites/Details/5
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

            return PartialView("_FavoriteDetail",favourite);
        }



        // GET: CustomerUser/Favorites/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: CustomerUser/Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoriteId,CustomerId,HomestayId,TourId,CreateDate")] Favourite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favorite.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // GET: CustomerUser/Favorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favourites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", favorite.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // POST: CustomerUser/Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoriteId,CustomerId,HomestayId,TourId,CreateDate")] Favourite favorite)
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", favorite.TourId);
            return View(favorite);
        }

        // GET: CustomerUser/Favorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: CustomerUser/Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favourites.FindAsync(id);
            if (favorite != null)
            {
                _context.Favourites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favourites.Any(e => e.FavoriteId == id);
        }
        [HttpPost]
        public IActionResult Remove(int id)
        {
            // Find the favourite item by its ID
            var favourite = _context.Favourites.FirstOrDefault(f => f.FavoriteId == id);

            if (favourite == null)
            {
                return NotFound();
            }

            // Remove the item from the database
            _context.Favourites.Remove(favourite);
            _context.SaveChanges();

            // Return a partial view or a success response
            return Json(new { success = true, message = "Item removed successfully!" });
        }
        [Authorize]
        public IActionResult PaymentCallBack() { return View(); }

    }
}
