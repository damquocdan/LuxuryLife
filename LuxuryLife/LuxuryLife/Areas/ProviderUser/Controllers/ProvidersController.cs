using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    [Area("ProviderUser")]
    public class ProvidersController : Controller
    {
        private readonly TourBookingContext _context;

        public ProvidersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers
                .FirstOrDefaultAsync(p => p.ProviderId == id);

            if (provider == null)
            {
                return NotFound();
            }

            // Fetch latest tours with images
            var tours = await _context.Tours
                .Include(t => t.Listimagestours)
                .Where(t => t.ProviderId == id && t.Status == "Active")
                .OrderByDescending(t => t.Createdate)
                .Take(3)
                .ToListAsync();

            // Fetch latest services
            var services = await _context.Services
                .Include(s => s.Tour)
                .Where(s => s.Tour.ProviderId == id)
                .OrderByDescending(s => s.Tour.Createdate)
                .Take(3)
                .ToListAsync();

            // Fetch booked customers
            var bookedCustomers = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .Where(b => b.Tour.ProviderId == id)
                .Select(b => new
                {
                    CustomerName = b.Customer.Name,
                    CustomerEmail = b.Customer.Email,
                    CustomerAvatar = b.Customer.Avatar ?? "/images/default-avatar.png", // Default avatar if null
                    TourName = b.Tour.Name,
                    BookingDate = b.BookingDate,
                    TotalPrice = b.TotalPrice
                })
                .OrderByDescending(b => b.BookingDate)
                .Take(5) // Limit to 5 customers
                .ToListAsync();

            ViewBag.Tours = tours;
            ViewBag.Services = services;
            ViewBag.BookedCustomers = bookedCustomers;

            return View(provider);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Edit", provider);
            }
            return View(provider);
        }

        // POST: AdminQL/Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProviderId,Name,Email,Password,Avatar,Phone,Address,Rating,Createdate")] Provider provider)
        {
            if (id != provider.ProviderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Any() && files[0].Length > 0)
                    {
                        var file = files[0];
                        var fileName = file.FileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\providers", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            provider.Avatar = "/images/providers/" + fileName;
                        }
                    }
                    _context.Update(provider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderExists(provider.ProviderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
            }
            return View(provider);
        }
        private bool ProviderExists(int id)
        {
            return _context.Providers.Any(e => e.ProviderId == id);
        }
    }
}