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
    public class CustomerSupportsController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public CustomerSupportsController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: AdminQL/CustomerSupports
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.CustomerSupports.Include(c => c.Customer);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: AdminQL/CustomerSupports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerSupport = await _context.CustomerSupports
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.SupportId == id);
            if (customerSupport == null)
            {
                return NotFound();
            }

            return View(customerSupport);
        }

        // GET: AdminQL/CustomerSupports/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            return View();
        }

        // POST: AdminQL/CustomerSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupportId,CustomerId,IssueDescription,SupportResponse,Status,CreateDate,ResolvedDate,IsResolved")] CustomerSupport customerSupport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerSupport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerSupport.CustomerId);
            return View(customerSupport);
        }

        // GET: AdminQL/CustomerSupports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerSupport = await _context.CustomerSupports.FindAsync(id);
            if (customerSupport == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerSupport.CustomerId);
            return View(customerSupport);
        }

        // POST: AdminQL/CustomerSupports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupportId,CustomerId,IssueDescription,SupportResponse,Status,CreateDate,ResolvedDate,IsResolved")] CustomerSupport customerSupport)
        {
            if (id != customerSupport.SupportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerSupport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerSupportExists(customerSupport.SupportId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", customerSupport.CustomerId);
            return View(customerSupport);
        }

        // GET: AdminQL/CustomerSupports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerSupport = await _context.CustomerSupports
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.SupportId == id);
            if (customerSupport == null)
            {
                return NotFound();
            }

            return View(customerSupport);
        }

        // POST: AdminQL/CustomerSupports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerSupport = await _context.CustomerSupports.FindAsync(id);
            if (customerSupport != null)
            {
                _context.CustomerSupports.Remove(customerSupport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerSupportExists(int id)
        {
            return _context.CustomerSupports.Any(e => e.SupportId == id);
        }
    }
}
