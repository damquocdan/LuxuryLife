using Microsoft.AspNetCore.Mvc;
using ASPNETCoreWebAPI.Models​;
using System.Linq;

namespace ASPNETCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public LoginController(TourBookingContext context)
        {
            _context = context;
        }

        // POST: api/Login/Register
        [HttpPost("Register")]
        public IActionResult Register([FromBody] Customer customer)
        {
            if (_context.Customers.Any(c => c.Email == customer.Email))
            {
                return BadRequest("Email is already registered.");
            }

            customer.Createdate = DateTime.UtcNow;
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok("Registration successful.");
        }

        // POST: api/Login
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Email == loginRequest.Email && c.Password == loginRequest.Password);

            if (customer == null)
            {
                return Unauthorized(new { error = "Invalid email or password." });
            }

            return Ok(new
            {
                Message = "Login successful.",
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email
            });
        }

    }

    // DTO for login request
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}