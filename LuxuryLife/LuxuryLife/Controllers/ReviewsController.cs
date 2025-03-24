using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly TourBookingContext _context;

        public ReviewsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.Reviews.Include(r => r.Customer).Include(r => r.Tour);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewId,CustomerId,TourId,Rating,Comment,Createdate")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", review.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", review.TourId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", review.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", review.TourId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,CustomerId,TourId,Rating,Comment,Createdate")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", review.CustomerId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", review.TourId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForTour(int TourId, string Comment, int Rating)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrWhiteSpace(Comment) || Rating < 1 || Rating > 5)
            {
                return RedirectToAction("Details", "Tours", new { id = TourId });
            }

            var review = new Review
            {
                TourId = TourId,
                CustomerId = customerId.Value,
                Comment = Comment,
                Rating = Rating,
                Createdate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Tours", new { id = TourId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReply(int ReviewId, string Comment)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrWhiteSpace(Comment))
            {
                var tourId = await _context.Reviews
                    .Where(r => r.ReviewId == ReviewId)
                    .Select(r => r.TourId)
                    .FirstOrDefaultAsync();
                return RedirectToAction("Details", "Tours", new { id = tourId });
            }

            var reviewOn = new ReviewOn
            {
                ReviewId = ReviewId,
                CustomerId = customerId.Value,
                Comment = Comment,
                Createdate = DateTime.Now
            };

            _context.ReviewOns.Add(reviewOn);
            await _context.SaveChangesAsync();

            var tourIdAfterSave = await _context.Reviews
                .Where(r => r.ReviewId == ReviewId)
                .Select(r => r.TourId)
                .FirstOrDefaultAsync();

            return RedirectToAction("Details", "Tours", new { id = tourIdAfterSave });
        }
        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
