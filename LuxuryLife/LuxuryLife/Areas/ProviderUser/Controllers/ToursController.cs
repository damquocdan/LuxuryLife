﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    public class ToursController : BaseController
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Tours
        public async Task<IActionResult> Index()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;

            if (providerId == 0)
            {
                return Unauthorized(); // Hoặc chuyển hướng đến trang đăng nhập
            }

            // Lấy danh sách các tour của nhà cung cấp
            var tours = await _context.Tours.Include(x=>x.Provider)
                .Where(t => t.ProviderId == providerId)
                .ToListAsync();

            return View(tours);
        }

        // GET: ProviderUser/Tours/Details/5
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

            // Truy vấn Service và Homestay theo khóa ngoại
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == tour.ServiceId);
            var homestay = await _context.Homestays.FirstOrDefaultAsync(h => h.HomestayId == tour.HomestayId);

            ViewBag.ServiceName = service?.ServiceName ?? "Không có dịch vụ";
            ViewBag.HomestayName = homestay?.Name ?? "Không có homestay";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", tour);
            }

            return View(tour);
        }
        public async Task<IActionResult> ListCustomers(int id)
        {
            var bookings = await _context.Bookings
                .Where(b => b.TourId == id)
                .Include(b => b.Customer)
                .Include(b => b.Tour)
                .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                ViewBag.Message = "Chưa có khách hàng nào đặt tour này.";
            }

            var tour = await _context.Tours.FindAsync(id);
            ViewBag.TourName = tour?.Name ?? "Không xác định";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ListCustomers", bookings);
            }
            return View(bookings);
        }
        public async Task<IActionResult> Reviews(int id)
        {
            var reviews = await _context.Reviews
                .Where(b => b.TourId == id)
                .Include(b => b.Customer)
                .Include(b => b.Tour).Include(b => b.ReviewOns)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                ViewBag.Message = "Chưa có khách hàng nào dánh giá tour này.";
            }

            var tour = await _context.Tours.FindAsync(id);
            ViewBag.TourName = tour?.Name ?? "Không xác định";

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Reviews", reviews);
            }
            return View(reviews);
        }
        // GET: ProviderUser/Tours/Create

        public IActionResult Create()
        {
            var providerId = HttpContext.Session.GetInt32("ProviderId");
            if (providerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["ProviderId"] = providerId.Value;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Create");
            }
            return View();
        }

        // POST: ProviderUser/Tours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Price,Status,Createdate,ProviderId")] Tour tour, int[] SelectedServiceIds, int[] SelectedHomestayIds)
        {
            var providerId = HttpContext.Session.GetInt32("ProviderId");
            if (providerId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            tour.ProviderId = providerId.Value;

            // Xử lý ngày và tính giá
            if (tour.StartDate.HasValue && tour.EndDate.HasValue)
            {
                if (tour.EndDate.Value >= tour.StartDate.Value)
                {
                    var daysDiff = (tour.EndDate.Value - tour.StartDate.Value).Days;
                    tour.Price = daysDiff * tour.PricePerson;
                }
                else
                {
                    ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Ngày bắt đầu và ngày kết thúc không được để trống.");
            }

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tour", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        tour.Image = "/images/tour/" + fileName;
                    }
                }
                else
                {
                    tour.Image = "/images/tour/default.png";
                }

 

                tour.Status = " ";
                tour.Createdate = DateTime.Now;

                _context.Add(tour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Truyền lại danh sách nếu form không hợp lệ
            ViewBag.Services = _context.Services
                .Where(s => s.Tour.ProviderId == providerId)
                .ToList();
            ViewBag.Homestays = _context.Homestays
                .Where(h => h.ProviderId == providerId)
                .ToList();
            ViewData["ProviderId"] = providerId.Value;
            return View(tour);
        }

        // GET: ProviderUser/Tours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var tour = await _context.Tours
                .Include(t => t.Services)
                .Include(t => t.Homestays)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TourId == id.Value);

            if (tour == null)
            {
                return NotFound();
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", tour.ProviderId);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Edit", tour);
            }
            return View(tour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,Name,Image,Description,ServiceId,HomestayId,PricePerson,StartDate,EndDate,Status,Createdate,ProviderId")] Tour tour)
        {
            if (id != tour.TourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTour = await _context.Tours.AsNoTracking().FirstOrDefaultAsync(a => a.TourId == tour.TourId);
                    if (existingTour == null)
                    {
                        return NotFound();
                    }

                    var files = HttpContext.Request.Form.Files;
                    if (files.Any() && files[0].Length > 0)
                    {
                        var file = files[0];
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tours");
                        Directory.CreateDirectory(uploadsFolder);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        tour.Image = "/images/tours/" + uniqueFileName;
                    }
                    else
                    {
                        tour.Image = existingTour.Image;
                    }

                    if (tour.Createdate == null)
                    {
                        tour.Createdate = existingTour.Createdate;
                    }

                    if (tour.ProviderId == null)
                    {
                        tour.ProviderId = existingTour.ProviderId;
                    }

                    else
                    {
                        tour.Price = existingTour.Price;
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
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Edit");
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", tour.ProviderId);
            return View(tour);
        }
        // GET: ProviderUser/Tours/Delete/5
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

        // POST: ProviderUser/Tours/Delete/5
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
