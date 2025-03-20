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
    public class ListimagestoursController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ListimagestoursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Listimagestours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Listimagestour>>> GetListimagestours()
        {
            return await _context.Listimagestours.ToListAsync();
        }

        // GET: api/Listimagestours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Listimagestour>> GetListimagestour(int id)
        {
            var listimagestour = await _context.Listimagestours.FindAsync(id);

            if (listimagestour == null)
            {
                return NotFound();
            }

            return listimagestour;
        }

        // PUT: api/Listimagestours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutListimagestour(int id, Listimagestour listimagestour)
        {
            if (id != listimagestour.ListimagestourId)
            {
                return BadRequest();
            }

            _context.Entry(listimagestour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListimagestourExists(id))
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

        // POST: api/Listimagestours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Listimagestour>> PostListimagestour(Listimagestour listimagestour)
        {
            _context.Listimagestours.Add(listimagestour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListimagestour", new { id = listimagestour.ListimagestourId }, listimagestour);
        }

        // DELETE: api/Listimagestours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListimagestour(int id)
        {
            var listimagestour = await _context.Listimagestours.FindAsync(id);
            if (listimagestour == null)
            {
                return NotFound();
            }

            _context.Listimagestours.Remove(listimagestour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListimagestourExists(int id)
        {
            return _context.Listimagestours.Any(e => e.ListimagestourId == id);
        }
    }
}
