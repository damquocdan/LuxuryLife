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
        private readonly TourBookingContext _context;

        public NewsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            // Lấy 9 tin tức mới nhất, sắp xếp theo Createdate giảm dần
            var latestNews = await _context.News
                .OrderByDescending(n => n.Createdate) // Sắp xếp theo ngày tạo giảm dần
                .Take(9) // Giới hạn 9 bản ghi
                .ToListAsync();

            return View(latestNews);
        }
        public IActionResult Details(int id)
        {
            var news = _context.News.FirstOrDefault(n => n.NewId == id);
            if (news == null)
            {
                return NotFound();
            }
            var relatedNews = _context.News
                .Where(n => n.NewId != id)
                .OrderByDescending(n => n.Createdate)
                .Take(3)
                .ToList();

            ViewBag.RelatedNews = relatedNews;
            return View(news);
        }
        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewId == id);
        }
    }
}
