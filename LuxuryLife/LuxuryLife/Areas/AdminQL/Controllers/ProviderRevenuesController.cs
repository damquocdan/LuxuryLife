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
    [Area("AdminQL")]
    public class ProviderRevenuesController : Controller
    {
        private readonly TourBookingContext _context;

        public ProviderRevenuesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: AdminQL/ProviderRevenues
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.ProviderRevenues.Include(p => p.Provider);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: AdminQL/ProviderRevenues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerRevenue = await _context.ProviderRevenues
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.RevenueId == id);
            if (providerRevenue == null)
            {
                return NotFound();
            }

            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Create
        public IActionResult Create()
        {
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email");
            return View();
        }

        // POST: AdminQL/ProviderRevenues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RevenueId,ProviderId,RevenueAmount,RevenueMonth,RevenueYear,CreatedAt,Status")] ProviderRevenue providerRevenue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(providerRevenue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerRevenue = await _context.ProviderRevenues.FindAsync(id);
            if (providerRevenue == null)
            {
                return NotFound();
            }
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // POST: AdminQL/ProviderRevenues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RevenueId,ProviderId,RevenueAmount,RevenueMonth,RevenueYear,CreatedAt,Status")] ProviderRevenue providerRevenue)
        {
            if (id != providerRevenue.RevenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(providerRevenue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProviderRevenueExists(providerRevenue.RevenueId))
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
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "Email", providerRevenue.ProviderId);
            return View(providerRevenue);
        }

        // GET: AdminQL/ProviderRevenues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerRevenue = await _context.ProviderRevenues
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(m => m.RevenueId == id);
            if (providerRevenue == null)
            {
                return NotFound();
            }

            return View(providerRevenue);
        }

        // POST: AdminQL/ProviderRevenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var providerRevenue = await _context.ProviderRevenues.FindAsync(id);
            if (providerRevenue != null)
            {
                _context.ProviderRevenues.Remove(providerRevenue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderRevenueExists(int id)
        {
            return _context.ProviderRevenues.Any(e => e.RevenueId == id);
        }
    }
}
