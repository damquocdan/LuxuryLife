using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebAPI.Models;

namespace ASPNETCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MomoPaymentsController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public MomoPaymentsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/MomoPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MomoPayment>>> GetMomoPayments()
        {
            return await _context.MomoPayments.ToListAsync();
        }

        // GET: api/MomoPayments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MomoPayment>> GetMomoPayment(int id)
        {
            var momoPayment = await _context.MomoPayments.FindAsync(id);

            if (momoPayment == null)
            {
                return NotFound();
            }

            return momoPayment;
        }

        // PUT: api/MomoPayments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMomoPayment(int id, MomoPayment momoPayment)
        {
            if (id != momoPayment.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(momoPayment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MomoPaymentExists(id))
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

        // POST: api/MomoPayments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MomoPayment>> PostMomoPayment(MomoPayment momoPayment)
        {
            _context.MomoPayments.Add(momoPayment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMomoPayment", new { id = momoPayment.PaymentId }, momoPayment);
        }

        // DELETE: api/MomoPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMomoPayment(int id)
        {
            var momoPayment = await _context.MomoPayments.FindAsync(id);
            if (momoPayment == null)
            {
                return NotFound();
            }

            _context.MomoPayments.Remove(momoPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MomoPaymentExists(int id)
        {
            return _context.MomoPayments.Any(e => e.PaymentId == id);
        }
    }
}
