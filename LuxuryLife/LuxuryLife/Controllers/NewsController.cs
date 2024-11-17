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
    public class NewsController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public NewsController(LuxuryLifeContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.News.Include(n => n.Author).Include(n => n.Homestay).Include(n => n.Provider).Include(n => n.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Author)
                .Include(n => n.Homestay)
                .Include(n => n.Provider)
                .Include(n => n.Tour)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Admins, "AdminId", "AdminId");
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId");
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId");
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,Title,Content,PublicationDate,AuthorId,HomestayId,TourId,ProviderId,CreateDate")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Admins, "AdminId", "AdminId", news.AuthorId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", news.HomestayId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", news.ProviderId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", news.TourId);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Admins, "AdminId", "AdminId", news.AuthorId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", news.HomestayId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", news.ProviderId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", news.TourId);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,Title,Content,PublicationDate,AuthorId,HomestayId,TourId,ProviderId,CreateDate")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsId))
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
            ViewData["AuthorId"] = new SelectList(_context.Admins, "AdminId", "AdminId", news.AuthorId);
            ViewData["HomestayId"] = new SelectList(_context.Homestays, "HomestayId", "HomestayId", news.HomestayId);
            ViewData["ProviderId"] = new SelectList(_context.Providers, "ProviderId", "ProviderId", news.ProviderId);
            ViewData["TourId"] = new SelectList(_context.Tours, "TourId", "TourId", news.TourId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Author)
                .Include(n => n.Homestay)
                .Include(n => n.Provider)
                .Include(n => n.Tour)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }
    }
}
