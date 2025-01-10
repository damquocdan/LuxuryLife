using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPITour.Models;

namespace ASPNETCoreWebAPITour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInteractionsController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public CustomerInteractionsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/CustomerInteractions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInteraction>>> GetCustomerInteractions()
        {
            return await _context.CustomerInteractions.ToListAsync();
        }

        // GET: api/CustomerInteractions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInteraction>> GetCustomerInteraction(int id)
        {
            var customerInteraction = await _context.CustomerInteractions.FindAsync(id);

            if (customerInteraction == null)
            {
                return NotFound();
            }

            return customerInteraction;
        }

        // PUT: api/CustomerInteractions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerInteraction(int id, CustomerInteraction customerInteraction)
        {
            if (id != customerInteraction.InteractionId)
            {
                return BadRequest();
            }

            _context.Entry(customerInteraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerInteractionExists(id))
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

        // POST: api/CustomerInteractions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerInteraction>> PostCustomerInteraction(CustomerInteraction customerInteraction)
        {
            _context.CustomerInteractions.Add(customerInteraction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerInteraction", new { id = customerInteraction.InteractionId }, customerInteraction);
        }

        // DELETE: api/CustomerInteractions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerInteraction(int id)
        {
            var customerInteraction = await _context.CustomerInteractions.FindAsync(id);
            if (customerInteraction == null)
            {
                return NotFound();
            }

            _context.CustomerInteractions.Remove(customerInteraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerInteractionExists(int id)
        {
            return _context.CustomerInteractions.Any(e => e.InteractionId == id);
        }
    }
}
