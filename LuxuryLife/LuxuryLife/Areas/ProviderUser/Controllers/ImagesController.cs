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
    public class ImagesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public ImagesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Images
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.Images.Include(i => i.Homestay).Include(i => i.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: ProviderUser/Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Homestay)
                .Include(i => i.Tour)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: ProviderUser/Images/Create
        public IActionResult Create()
        {
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: ProviderUser/Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,HomestayId,TourId,ImageUrl,ImageDescription,CreateDate")] Image image)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", image.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", image.TourId);
            return View(image);
        }

        // GET: ProviderUser/Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", image.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", image.TourId);
            return View(image);
        }

        // POST: ProviderUser/Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,HomestayId,TourId,ImageUrl,ImageDescription,CreateDate")] Image image)
        {
            if (id != image.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.ImageId))
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
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", image.HomestayId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", image.TourId);
            return View(image);
        }

        // GET: ProviderUser/Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Homestay)
                .Include(i => i.Tour)
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: ProviderUser/Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
