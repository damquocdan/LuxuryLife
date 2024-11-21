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
    public class TourSchedulesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public TourSchedulesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/TourSchedules
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.TourSchedules.Include(t => t.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: ProviderUser/TourSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourSchedule = await _context.TourSchedules
                .Include(t => t.Tour)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (tourSchedule == null)
            {
                return NotFound();
            }

            return View(tourSchedule);
        }

        // GET: ProviderUser/TourSchedules/Create
        public IActionResult Create()
        {
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: ProviderUser/TourSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,TourId,Day,ActivityDescription,Location,ScheduleDate")] TourSchedule tourSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tourSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourSchedule.TourId);
            return View(tourSchedule);
        }

        // GET: ProviderUser/TourSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourSchedule = await _context.TourSchedules.FindAsync(id);
            if (tourSchedule == null)
            {
                return NotFound();
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourSchedule.TourId);
            return View(tourSchedule);
        }

        // POST: ProviderUser/TourSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,TourId,Day,ActivityDescription,Location,ScheduleDate")] TourSchedule tourSchedule)
        {
            if (id != tourSchedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tourSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourScheduleExists(tourSchedule.ScheduleId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", tourSchedule.TourId);
            return View(tourSchedule);
        }

        // GET: ProviderUser/TourSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourSchedule = await _context.TourSchedules
                .Include(t => t.Tour)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (tourSchedule == null)
            {
                return NotFound();
            }

            return View(tourSchedule);
        }

        // POST: ProviderUser/TourSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourSchedule = await _context.TourSchedules.FindAsync(id);
            if (tourSchedule != null)
            {
                _context.TourSchedules.Remove(tourSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourScheduleExists(int id)
        {
            return _context.TourSchedules.Any(e => e.ScheduleId == id);
        }
    }
}
