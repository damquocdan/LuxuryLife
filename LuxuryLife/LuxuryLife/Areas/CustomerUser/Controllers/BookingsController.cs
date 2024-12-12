using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    [Area("CustomerUser")]
    public class BookingsController : Controller
    {
        private readonly TourBookingContext _context;

        public BookingsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: CustomerUser/Bookings
        public async Task<IActionResult> Index()
        {
            var tourbookingContext = _context.Bookings.Include(b => b.Customer).Include(b => b.Tour);
            return View(await tourbookingContext.ToListAsync());
        }

        // GET: CustomerUser/Bookings/Details/5
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

        // GET: CustomerUser/Bookings/Create
        [HttpGet]
        public IActionResult Create(int id, int customerId)
        {
            // Lấy thông tin tour từ cơ sở dữ liệu
            var tour = _context.Tours.FirstOrDefault(t => t.TourId == id);
            if (tour == null)
            {
                return NotFound("Tour không tồn tại");
            }

            // Tạo đối tượng Booking với giá trị mặc định từ Tour
            var booking = new Booking
            {
                TourId = id,
                CustomerId = customerId,
                CheckInDate = tour.StartDate,
                CheckOutDate = tour.EndDate,
                Status = "Pending", // Giá trị mặc định
                TotalPrice = tour.Price
            };

            // Truyền thêm thông tin để hiển thị
            ViewBag.TourName = tour.Name;
            ViewBag.CustomerName = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId)?.Name;
            ViewBag.TotalPrice= tour.Price;

            return View(booking);
        }



        // POST: CustomerUser/Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", booking.TourId);
            return View(booking);
        }

        // GET: CustomerUser/Bookings/Edit/5
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", booking.TourId);
            return View(booking);
        }

        // POST: CustomerUser/Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerId,TourId,BookingDate,CheckInDate,CheckOutDate,Status,TotalPrice")] Booking booking)
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", booking.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", booking.TourId);
            return View(booking);
        }

        // GET: CustomerUser/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: CustomerUser/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
