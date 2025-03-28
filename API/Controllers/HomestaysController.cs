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
    public class HomestaysController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public HomestaysController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Homestays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Homestay>>> GetHomestays()
        {
            var homestays = await _context.Homestays
                .Include(h => h.Tour)
                .ThenInclude(t => t.Provider)
                .ToListAsync();

            return Ok(homestays);
        }

        // GET: api/Homestays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Homestay>> GetHomestay(int id)
        {
            var homestay = await _context.Homestays
                .Include(h => h.Tour)
                .ThenInclude(t => t.Provider)
                .FirstOrDefaultAsync(m => m.HomestayId == id);

            if (homestay == null)
            {
                return NotFound();
            }

            return Ok(homestay);
        }

        private bool HomestayExists(int id)
        {
            return _context.Homestays.Any(e => e.HomestayId == id);
        }
    }
}