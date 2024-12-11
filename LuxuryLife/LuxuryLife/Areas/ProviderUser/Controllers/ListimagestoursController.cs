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
    public class ListimagestoursController : BaseController
    {
        private readonly TourbookingContext _context;

        public ListimagestoursController(TourbookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Listimagestours
        public async Task<IActionResult> Index()
        {
            var tourbookingContext = _context.Listimagestours.Include(l => l.Tour);
            return View(await tourbookingContext.ToListAsync());
        }

        // GET: ProviderUser/Listimagestours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours
                .Include(l => l.Tour)
                .FirstOrDefaultAsync(m => m.ListimagestourId == id);
            if (listimagestour == null)
            {
                return NotFound();
            }

            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Create
        public IActionResult Create()
        {
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: ProviderUser/Listimagestours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListimagestourId,TourId,ImageUrl,ImageDescription,Createdate")] Listimagestour listimagestour)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\listimagetour", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        listimagestour.ImageUrl = "/images/listimagetour/" + FileName;
                    }
                }
                _context.Add(listimagestour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", listimagestour.TourId);
            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours.FindAsync(id);
            if (listimagestour == null)
            {
                return NotFound();
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", listimagestour.TourId);
            return View(listimagestour);
        }

        // POST: ProviderUser/Listimagestours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListimagestourId,TourId,ImageUrl,ImageDescription,Createdate")] Listimagestour listimagestour)
        {
            if (id != listimagestour.ListimagestourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listimagestour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListimagestourExists(listimagestour.ListimagestourId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", listimagestour.TourId);
            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours
                .Include(l => l.Tour)
                .FirstOrDefaultAsync(m => m.ListimagestourId == id);
            if (listimagestour == null)
            {
                return NotFound();
            }

            return View(listimagestour);
        }

        // POST: ProviderUser/Listimagestours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listimagestour = await _context.Listimagestours.FindAsync(id);
            if (listimagestour != null)
            {
                _context.Listimagestours.Remove(listimagestour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListimagestourExists(int id)
        {
            return _context.Listimagestours.Any(e => e.ListimagestourId == id);
        }
    }
}
