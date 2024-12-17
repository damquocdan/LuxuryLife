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
    public class ListimagestoursController : BaseController
    { 
        private readonly TourBookingContext _context;

        public ListimagestoursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Listimagestours
        public async Task<IActionResult> Index()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var tourbookingContext = _context.Listimagestours.Include(x=>x.Tour).Where(l => l.Tour.ProviderId==providerId);
            return View(await tourbookingContext.ToListAsync());
        }

        // GET: ProviderUser/Listimagestours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours
                .Include(l => l.Tour)
                .FirstOrDefaultAsync(m => m.ListimagestourId == id);
            if (listimagestour == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Details", listimagestour);
            }
            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Create
        public IActionResult Create()
        {
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var providerTours = _context.Tours.Where(t => t.ProviderId == providerId).ToList();
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
                return PartialView("_Create");
            }
            // Create SelectList with filtered tours
            ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
            return View();
        }

        // POST: ProviderUser/Listimagestours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListimagestourId,TourId,ImageUrl,ImageDescription,Createdate")] Listimagestour listimagestour)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0)
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\listimagetour", FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        listimagestour.ImageUrl = "/images/listimagetour/" + FileName;
                    }
                }
                _context.Add(listimagestour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "Name", listimagestour.TourId);
            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours.FindAsync(id);
            if (listimagestour == null)
            {
                return NotFound();
            }
            int providerId = HttpContext.Session.GetInt32("ProviderId") ?? 0;
            var providerTours = _context.Tours.Where(t => t.ProviderId == providerId).ToList();
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                ViewData["TourId"] = new SelectList(providerTours, "TourId", "Name");
                return PartialView("_Edit", listimagestour);
            }
            
            return View(listimagestour);
        }

        // POST: ProviderUser/Listimagestours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListimagestourId,TourId,ImageUrl,ImageDescription,Createdate")] Listimagestour listimagestour)
        {
            if (id != listimagestour.ListimagestourId)
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
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/listimagetour");
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
                        listimagestour.ImageUrl = "/images/listimagetour/" + uniqueFileName;
                    }
                    else
                    {
                        // Nếu không upload file mới, giữ nguyên ảnh cũ
                        var existingImage = await _context.Listimagestours.AsNoTracking().FirstOrDefaultAsync(a => a.ListimagestourId == listimagestour.ListimagestourId);
                        if (existingImage != null)
                        {
                            listimagestour.ImageUrl = existingImage.ImageUrl;
                        }
                    }
                    _context.Update(listimagestour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListimagestourExists(listimagestour.ListimagestourId))
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
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", listimagestour.TourId);
            return View(listimagestour);
        }

        // GET: ProviderUser/Listimagestours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listimagestour = await _context.Listimagestours
                .Include(l => l.Tour)
                .FirstOrDefaultAsync(m => m.ListimagestourId == id);
            if (listimagestour == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Delete", listimagestour);
            }
            return View(listimagestour);
        }

        // POST: ProviderUser/Listimagestours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listimagestour = await _context.Listimagestours.FindAsync(id);
            if (listimagestour != null)
            {
                _context.Listimagestours.Remove(listimagestour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListimagestourExists(int id)
        {
            return _context.Listimagestours.Any(e => e.ListimagestourId == id);
        }
    }
}
