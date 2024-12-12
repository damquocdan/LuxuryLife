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
    public class HomestaysController : Controller
    {
        private readonly TourBookingContext _context;

        public HomestaysController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Homestays
        public async Task<IActionResult> Index()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;

            if (providerId == 0)
            {
                return Unauthorized(); // Hoặc chuyển hướng đến trang đăng nhập
            }

            // Lấy danh sách các tour của nhà cung cấp
            var homestays = await _context.Homestays
                .Where(t => t.ProviderId == providerId)
                .ToListAsync();

            return View(homestays);
        }

        // GET: ProviderUser/Homestays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homestay = await _context.Homestays
                .FirstOrDefaultAsync(m => m.HomestayId == id);
            if (homestay == null)
            {
                return NotFound();
            }

            return View(homestay);
        }

        // GET: ProviderUser/Homestays/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            return View();
        }

        // POST: ProviderUser/Homestays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HomestayId,Name,Description,Address,Image,PricePerNight,ProviderId")] Homestay homestay)
        {
            homestay.ProviderId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            if (ModelState.IsValid)
            {
                _context.Add(homestay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homestay);
        }

        // GET: ProviderUser/Homestays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homestay = await _context.Homestays.FindAsync(id);
            if (homestay == null)
            {
                return NotFound();
            }
            return View(homestay);
        }

        // POST: ProviderUser/Homestays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HomestayId,Name,Description,Address,Image,PricePerNight,ProviderId")] Homestay homestay)
        {
            if (id != homestay.HomestayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homestay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomestayExists(homestay.HomestayId))
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
            return View(homestay);
        }

        // GET: ProviderUser/Homestays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homestay = await _context.Homestays
                .FirstOrDefaultAsync(m => m.HomestayId == id);
            if (homestay == null)
            {
                return NotFound();
            }

            return View(homestay);
        }

        // POST: ProviderUser/Homestays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homestay = await _context.Homestays.FindAsync(id);
            if (homestay != null)
            {
                _context.Homestays.Remove(homestay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomestayExists(int id)
        {
            return _context.Homestays.Any(e => e.HomestayId == id);
        }
    }
}
