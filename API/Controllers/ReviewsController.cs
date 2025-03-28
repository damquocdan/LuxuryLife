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
    public class ReviewsController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ReviewsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Tour)
                .ToListAsync();

            return Ok(reviews);
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Tour)
                .FirstOrDefaultAsync(m => m.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Reviews/CreateForTour
        [HttpPost("CreateForTour")]
        public async Task<IActionResult> CreateForTour([FromBody] CreateReviewForTourRequest request)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return Unauthorized(new { message = "User not logged in" });
            }

            if (string.IsNullOrWhiteSpace(request.Comment) || request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest(new { message = "Invalid comment or rating. Rating must be between 1 and 5." });
            }

            var review = new Review
            {
                TourId = request.TourId,
                CustomerId = customerId.Value,
                Comment = request.Comment,
                Rating = request.Rating,
                Createdate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
        }

        // POST: api/Reviews/AddReply
        [HttpPost("AddReply")]
        public async Task<IActionResult> AddReply([FromBody] AddReplyRequest request)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return Unauthorized(new { message = "User not logged in" });
            }

            if (string.IsNullOrWhiteSpace(request.Comment))
            {
                return BadRequest(new { message = "Comment cannot be empty" });
            }

            var reviewOn = new ReviewOn
            {
                ReviewId = request.ReviewId,
                CustomerId = customerId.Value,
                Comment = request.Comment,
                Createdate = DateTime.Now
            };

            _context.ReviewOns.Add(reviewOn);
            await _context.SaveChangesAsync();

            return Ok(reviewOn);
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }

    // Request models for custom POST actions
    public class CreateReviewForTourRequest
    {
        public int TourId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }

    public class AddReplyRequest
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
    }
}