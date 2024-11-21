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
    public class BookingServicesController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public BookingServicesController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: AdminQL/BookingServices
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.BookingServices.Include(b => b.Booking);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: AdminQL/BookingServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices
                .Include(b => b.Booking)
                .FirstOrDefaultAsync(m => m.BookingServiceId == id);
            if (bookingService == null)
            {
                return NotFound();
            }

            return View(bookingService);
        }

        // GET: AdminQL/BookingServices/Create
        public IActionResult Create()
        {
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId");
            return View();
        }

        // POST: AdminQL/BookingServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingServiceId,BookingId,ServiceName,ServicePrice")] BookingService bookingService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            return View(bookingService);
        }

        // GET: AdminQL/BookingServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices.FindAsync(id);
            if (bookingService == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            return View(bookingService);
        }

        // POST: AdminQL/BookingServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingServiceId,BookingId,ServiceName,ServicePrice")] BookingService bookingService)
        {
            if (id != bookingService.BookingServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingServiceExists(bookingService.BookingServiceId))
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
            ViewData["BookingId"] = new SelectList(_context.Bookings, "BookingId", "BookingId", bookingService.BookingId);
            return View(bookingService);
        }

        // GET: AdminQL/BookingServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingService = await _context.BookingServices
                .Include(b => b.Booking)
                .FirstOrDefaultAsync(m => m.BookingServiceId == id);
            if (bookingService == null)
            {
                return NotFound();
            }

            return View(bookingService);
        }

        // POST: AdminQL/BookingServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingService = await _context.BookingServices.FindAsync(id);
            if (bookingService != null)
            {
                _context.BookingServices.Remove(bookingService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingServiceExists(int id)
        {
            return _context.BookingServices.Any(e => e.BookingServiceId == id);
        }
    }
}
