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
    public class ServicesController : BaseController
    {
        private readonly TourBookingContext _context;

        public ServicesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Services
        public async Task<IActionResult> Index()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var tourBookingContext = await _context.Services.Include(x=>x.Tour).Where(s => s.Tour.ProviderId == providerId).ToListAsync();
            return View(tourBookingContext);
        }

        // GET: ProviderUser/Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", service);
            }
            return View(service);
        }

        // GET: ProviderUser/Services/Create
        public IActionResult Create()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var providerTours = _context.Tours.Where(t => t.ProviderId == providerId).ToList();
            ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
                return PartialView("_Create");
            }
            return View();
        }

        // POST: ProviderUser/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,Price,TourId")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", service.TourId);
            return View(service);
        }

        // GET: ProviderUser/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var providerTours = _context.Tours.Where(t => t.ProviderId == providerId).ToList();
            ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
                return PartialView("_Edit", service);
            }
            return View(service);
        }

        // POST: ProviderUser/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,Price,TourId")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", service.TourId);
            return View(service);
        }

        // GET: ProviderUser/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Tour)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Delete", service);
            }
            return View(service);
        }

        // POST: ProviderUser/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }
    }
}
