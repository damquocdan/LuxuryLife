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
