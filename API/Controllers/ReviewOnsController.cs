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
    public class ReviewOnsController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ReviewOnsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/ReviewOns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewOn>>> GetReviewOns()
        {
            var reviewOns = await _context.ReviewOns
                .Include(r => r.Customer)
                .Include(r => r.Review)
                .ToListAsync();

            return Ok(reviewOns);
        }

        // GET: api/ReviewOns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewOn>> GetReviewOn(int id)
        {
            var reviewOn = await _context.ReviewOns
                .Include(r => r.Customer)
                .Include(r => r.Review)
                .FirstOrDefaultAsync(m => m.ReviewOnId == id);

            if (reviewOn == null)
            {
                return NotFound();
            }

            return Ok(reviewOn);
        }

        // POST: api/ReviewOns
        [HttpPost]
        public async Task<ActionResult<ReviewOn>> PostReviewOn(ReviewOn reviewOn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ReviewOns.Add(reviewOn);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReviewOn), new { id = reviewOn.ReviewOnId }, reviewOn);
        }

        // PUT: api/ReviewOns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReviewOn(int id, ReviewOn reviewOn)
        {
            if (id != reviewOn.ReviewOnId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(reviewOn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewOnExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ReviewOns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewOn(int id)
        {
            var reviewOn = await _context.ReviewOns.FindAsync(id);
            if (reviewOn == null)
            {
                return NotFound();
            }

            _context.ReviewOns.Remove(reviewOn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewOnExists(int id)
        {
            return _context.ReviewOns.Any(e => e.ReviewOnId == id);
        }
    }
}