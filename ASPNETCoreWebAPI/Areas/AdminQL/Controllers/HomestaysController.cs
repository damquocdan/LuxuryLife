using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPI.Models;

namespace ASPNETCoreWebAPI.Areas.AdminQL.Controllers
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
            return await _context.Homestays.ToListAsync();
        }

        // GET: api/Homestays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Homestay>> GetHomestay(int id)
        {
            var homestay = await _context.Homestays.FindAsync(id);

            if (homestay == null)
            {
                return NotFound();
            }

            return homestay;
        }

        // PUT: api/Homestays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomestay(int id, Homestay homestay)
        {
            if (id != homestay.HomestayId)
            {
                return BadRequest();
            }

            _context.Entry(homestay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomestayExists(id))
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

        // POST: api/Homestays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Homestay>> PostHomestay(Homestay homestay)
        {
            _context.Homestays.Add(homestay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomestay", new { id = homestay.HomestayId }, homestay);
        }

        // DELETE: api/Homestays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHomestay(int id)
        {
            var homestay = await _context.Homestays.FindAsync(id);
            if (homestay == null)
            {
                return NotFound();
            }

            _context.Homestays.Remove(homestay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HomestayExists(int id)
        {
            return _context.Homestays.Any(e => e.HomestayId == id);
        }
    }
}
