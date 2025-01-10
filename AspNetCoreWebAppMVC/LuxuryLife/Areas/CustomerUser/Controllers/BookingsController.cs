using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class BookingsController : BaseController
    {
        private readonly TourBookingContext _context;

        public BookingsController(TourBookingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            var bookings = _context.Bookings.Include(b => b.Customer).Include(b => b.Tour).Where(b => b.CustomerId == customerId);
            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpGet]
        public IActionResult Create(int id, int customerId)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.TourId == id);
            if (tour == null) return NotFound("Tour không tồn tại");

            var booking = new Booking
            {
                TourId = id,
                CustomerId = customerId,
                CheckInDate = tour.StartDate,
                CheckOutDate = tour.EndDate,
                Status = "Pending",
                TotalPrice = tour.Price
            };

            ViewBag.TourName = tour.Name;
            ViewBag.CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId)?.Name;
            ViewBag.TotalPrice = tour.Price;
            ViewBag.CheckInDate = tour.StartDate;
            ViewBag.CheckOutDate = tour.EndDate;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            var existingBooking = _context.Bookings
                                          .FirstOrDefault(b => b.TourId == booking.TourId && b.CustomerId == booking.CustomerId);

            if (existingBooking != null)
            {
                ModelState.AddModelError("", "Bạn đã đặt tour này rồi, không thể đặt lại.");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();

                var favourite = _context.Favourites
                                        .FirstOrDefault(f => f.TourId == booking.TourId && f.CustomerId == booking.CustomerId);
                if (favourite != null)
                {
                    _context.Favourites.Remove(favourite);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            if (id != booking.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null) _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}