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
    public class BookingsController : BaseController
    {
        private readonly TourBookingContext _context;

        public BookingsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Bookings
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Tour);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: AdminQL/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: AdminQL/Bookings/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Name");
            return View();
        }

        // POST: AdminQL/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,TotalPrice,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Name", booking.TourId);
            return View(booking);
        }

        // GET: AdminQL/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Name", booking.TourId);
            return View(booking);
        }

        // POST: AdminQL/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,TotalPrice,Status")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Name", booking.TourId);
            return View(booking);
        }

        // GET: AdminQL/Bookings/Delete/5
        // POST: AdminQL/Bookings/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
        [HttpGet]
        public async Task<IActionResult> EditStatus(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Thay đổi trạng thái booking
            booking.Status = booking.Status == "Cancelled" ? "Confirmed" : "Cancelled";

            // Lưu thay đổi
            _context.Update(booking);
            await _context.SaveChangesAsync();

            // Điều hướng lại về trang danh sách booking
            return RedirectToAction(nameof(Index));
        }
    }
}
