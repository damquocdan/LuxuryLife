using System;
using System.Collections.Generic;
using System.IO;
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
    public class CustomersController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public CustomersController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Lấy danh sách đánh giá
            var reviews = await _context.Reviews
                .Where(r => r.CustomerId == id)
                .Include(r => r.Tour)
                .Select(r => new
                {
                    r.ReviewId,
                    r.Rating,
                    r.Comment,
                    r.Createdate,
                    Tour = new
                    {
                        r.Tour.Name,
                        r.Tour.Image
                    }
                })
                .OrderByDescending(r => r.Createdate)
                .ToListAsync();

            // Lấy danh sách tour đã đặt
            var bookedTours = await _context.Bookings
                .Where(b => b.CustomerId == id)
                .Include(b => b.Tour)
                .Select(b => new
                {
                    b.Tour.Name,
                    b.Tour.Image,
                    b.BookingDate
                })
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync();

            var response = new
            {
                Customer = customer,
                Reviews = reviews,
                BookedTours = bookedTours
            };

            return Ok(response);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromForm] Customer customer, IFormFile? avatarFile)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest(new { message = "Customer ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/customers");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(avatarFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    customer.Avatar = "/images/customers/" + uniqueFileName;
                }
                else
                {
                    var existingCustomer = await _context.Customers.AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
                    if (existingCustomer != null)
                    {
                        customer.Avatar = existingCustomer.Avatar;
                    }
                }

                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}