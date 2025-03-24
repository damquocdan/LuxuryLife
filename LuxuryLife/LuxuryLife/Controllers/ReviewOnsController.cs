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
    public class ReviewOnsController : Controller
    {
        private readonly TourBookingContext _context;

        public ReviewOnsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ReviewOns
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.ReviewOns.Include(r => r.Customer).Include(r => r.Review);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: ReviewOns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewOn = await _context.ReviewOns
                .Include(r => r.Customer)
                .Include(r => r.Review)
                .FirstOrDefaultAsync(m => m.ReviewOnId == id);
            if (reviewOn == null)
            {
                return NotFound();
            }

            return View(reviewOn);
        }

        // GET: ReviewOns/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email");
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "ReviewId", "ReviewId");
            return View();
        }

        // POST: ReviewOns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewOnId,ReviewId,CustomerId,Comment,Createdate")] ReviewOn reviewOn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviewOn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", reviewOn.CustomerId);
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "ReviewId", "ReviewId", reviewOn.ReviewId);
            return View(reviewOn);
        }

        // GET: ReviewOns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewOn = await _context.ReviewOns.FindAsync(id);
            if (reviewOn == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", reviewOn.CustomerId);
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "ReviewId", "ReviewId", reviewOn.ReviewId);
            return View(reviewOn);
        }

        // POST: ReviewOns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewOnId,ReviewId,CustomerId,Comment,Createdate")] ReviewOn reviewOn)
        {
            if (id != reviewOn.ReviewOnId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewOn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewOnExists(reviewOn.ReviewOnId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", reviewOn.CustomerId);
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "ReviewId", "ReviewId", reviewOn.ReviewId);
            return View(reviewOn);
        }

        // GET: ReviewOns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewOn = await _context.ReviewOns
                .Include(r => r.Customer)
                .Include(r => r.Review)
                .FirstOrDefaultAsync(m => m.ReviewOnId == id);
            if (reviewOn == null)
            {
                return NotFound();
            }

            return View(reviewOn);
        }

        // POST: ReviewOns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewOn = await _context.ReviewOns.FindAsync(id);
            if (reviewOn != null)
            {
                _context.ReviewOns.Remove(reviewOn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewOnExists(int id)
        {
            return _context.ReviewOns.Any(e => e.ReviewOnId == id);
        }
    }
}
