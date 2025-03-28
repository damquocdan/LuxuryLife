using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public FavouritesController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetFavourites()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return Unauthorized(new { message = "You need to log in to access your favorites." });
            }

            var favourites = await _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .ThenInclude(t => t.Provider)
                .Where(f => f.CustomerId == customerId)
                .ToListAsync();

            return Ok(favourites);
        }

        // GET: api/Favourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favourite>> GetFavourite(int id)
        {
            var favourite = await _context.Favourites
                .Include(f => f.Customer)
                .Include(f => f.Tour)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);

            if (favourite == null)
            {
                return NotFound();
            }

            return Ok(favourite);
        }

        // POST: api/Favourites/Add?tourId=5
        [HttpPost("Add")]
        public async Task<IActionResult> AddFavourite([FromQuery] int tourId)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return Unauthorized(new { message = "You need to log in to add a favorite." });
            }

            var existingFavorite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.TourId == tourId && f.CustomerId == customerId);

            if (existingFavorite != null)
            {
                return BadRequest(new { message = "This tour is already in your favorites!" });
            }

            var favorite = new Favourite
            {
                CustomerId = customerId.Value,
                TourId = tourId
            };

            _context.Favourites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Tour added to favorites!", favorite });
        }

        // POST: api/Favourites
        [HttpPost]
        public async Task<ActionResult<Favourite>> PostFavourite(Favourite favourite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFavourite), new { id = favourite.FavoriteId }, favourite);
        }

        // PUT: api/Favourites/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (id != favourite.FavoriteId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(favourite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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

        // DELETE: api/Favourites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Favourites/Remove?id=5
        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveFavourite([FromQuery] int id)
        {
            var favourite = await _context.Favourites.FirstOrDefaultAsync(f => f.FavoriteId == id);

            if (favourite == null)
            {
                return NotFound(new { success = false, message = "Favorite not found." });
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Item removed successfully!" });
        }

        // GET: api/Favourites/PaymentCallBack
        [Authorize]
        [HttpGet("PaymentCallBack")]
        public IActionResult PaymentCallBack()
        {
            // This endpoint is kept simple as the original just returned a view
            return Ok(new { message = "Payment callback processed." });
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favourites.Any(e => e.FavoriteId == id);
        }
    }
}