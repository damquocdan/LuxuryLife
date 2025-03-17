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
    public class ReviewsController : BaseController
    {
        private readonly TourBookingContext _context;

        public ReviewsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: ProviderUser/Reviews
        public async Task<IActionResult> Index()
        {
            var luxuryLifeContext = _context.Reviews.Include(r => r.Customer).Include(r => r.Tour);
            return View(await luxuryLifeContext.ToListAsync());
        }

        // GET: ProviderUser/Reviews/Details/5
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
        
        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
