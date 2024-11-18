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
    public class TourPackagesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public TourPackagesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/TourPackages
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.TourPackages.Include(t => t.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: ProviderUser/TourPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourPackage = await _context.TourPackages
                .Include(t => t.Tour)
                .FirstOrDefaultAsync(m => m.PackageId == id);
            if (tourPackage == null)
            {
                return NotFound();
            }

            return View(tourPackage);
        }

        // GET: ProviderUser/TourPackages/Create
        public IActionResult Create()
        {
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: ProviderUser/TourPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PackageId,TourId,PackageName,PackagePrice,Description")] TourPackage tourPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tourPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourPackage.TourId);
            return View(tourPackage);
        }

        // GET: ProviderUser/TourPackages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourPackage = await _context.TourPackages.FindAsync(id);
            if (tourPackage == null)
            {
                return NotFound();
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourPackage.TourId);
            return View(tourPackage);
        }

        // POST: ProviderUser/TourPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PackageId,TourId,PackageName,PackagePrice,Description")] TourPackage tourPackage)
        {
            if (id != tourPackage.PackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tourPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourPackageExists(tourPackage.PackageId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourPackage.TourId);
            return View(tourPackage);
        }

        // GET: ProviderUser/TourPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourPackage = await _context.TourPackages
                .Include(t => t.Tour)
                .FirstOrDefaultAsync(m => m.PackageId == id);
            if (tourPackage == null)
            {
                return NotFound();
            }

            return View(tourPackage);
        }

        // POST: ProviderUser/TourPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourPackage = await _context.TourPackages.FindAsync(id);
            if (tourPackage != null)
            {
                _context.TourPackages.Remove(tourPackage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourPackageExists(int id)
        {
            return _context.TourPackages.Any(e => e.PackageId == id);
        }
    }
}
