using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public NewsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            // Lấy 9 tin tức mới nhất, sắp xếp theo Createdate giảm dần
            var latestNews = await _context.News
                .OrderByDescending(n => n.Createdate) // Sắp xếp theo ngày tạo giảm dần
                .Take(9) // Giới hạn 9 bản ghi
                .ToListAsync();

            return Ok(latestNews);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetNewsDetails(int id)
        {
            // Fetch the news item asynchronously
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.NewId == id);

            if (news == null)
            {
                return NotFound();
            }

            // Fetch related news (top 3 excluding the current one)
            var relatedNews = await _context.News
                .Where(n => n.NewId != id)
                .OrderByDescending(n => n.Createdate)
                .Take(3)
                .ToListAsync();

            // Construct the response object
            var response = new
            {
                News = news,
                RelatedNews = relatedNews
            };

            return Ok(response);
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewId == id);
        }
    }
}