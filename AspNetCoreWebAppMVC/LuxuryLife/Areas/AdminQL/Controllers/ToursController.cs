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
    public class ToursController : BaseController
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Tours
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.Tours.Include(t => t.Provider);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: AdminQL/Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Provider)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", tour);
            }

            return View(tour);
        }

        // GET: AdminQL/Tours/Create
        public IActionResult Create()
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Create");
                ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email");
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email");
            return View();
        }

        // POST: AdminQL/Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Price,Status,Createdate,ProviderId")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Any() && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\tours", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        tour.Image = "/images/tours/" + fileName;
                    }
                }
                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Name", tour.ProviderId);
            return View(tour);
        }

        // GET: AdminQL/Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Name", tour.ProviderId);
                return PartialView("_Edit", tour);
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Name", tour.ProviderId);
            return View(tour);
        }

        // POST: AdminQL/Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Price,Status,Createdate,ProviderId")] Tour tour)
        {
            if (id != tour.TourId)
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

                        // Đảm bảo thư mục lưu trữ tồn tại
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tour");
                        Directory.CreateDirectory(uploadsFolder);

                        // Tạo tên file duy nhất để tránh xung đột
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Lưu file vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn Avatar
                        tour.Image = "/images/tour/" + uniqueFileName;
                    }
                    else
                    {
                        // Nếu không upload file mới, giữ nguyên ảnh cũ
                        var existingTour = await _context.Tours.AsNoTracking().FirstOrDefaultAsync(a => a.TourId == tour.TourId);
                        if (existingTour != null)
                        {
                            tour.Image = existingTour.Image;
                        }
                    }
                    _context.Update(tour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TourExists(tour.TourId))
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
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", tour.ProviderId);
            return View(tour);
        }

        // GET: AdminQL/Tours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Provider)
                .FirstOrDefaultAsync(m => m.TourId == id);
            if (tour == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Delete", tour);
            }
            return View(tour);
        }

        // POST: AdminQL/Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}
