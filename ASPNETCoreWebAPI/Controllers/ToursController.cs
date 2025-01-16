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
    public class ToursController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Tours
        [HttpGet("Index")]
        public async Task<ActionResult<IEnumerable<Tour>>> GetTours()
        {
            var tours = await _context.Tours
                                      .Include(t => t.Provider)  // Eager load the related provider
                                      .Where(t => t.Status == "Active")  // Filter by active status
                                      .ToListAsync();

            var result = tours.Select(t => new
            {
                t.TourId,
                t.Name,
                t.Description,
                t.Image,
                t.Price,
                t.Status,
                ProviderName = t.Provider.Name,
                ProviderAvatar = t.Provider.Avatar
            });

            return Ok(result);  // Return the data as a JSON response
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourDetails(int? id)
        {
            if (id == null)
            {
                return NotFound(new { message = "Tour not found" });
            }

            var tour = await _context.Tours
                .Include(t => t.Provider)
                .Include(l => l.Listimagestours)
                .Include(t => t.Services)
                .Include(c => c.Homestays)
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(m => m.TourId == id);

            if (tour == null)
            {
                return NotFound(new { message = "Tour not found" });
            }

            // Prepare a custom response to include the relevant data for the frontend
            var result = new
            {
                tour.TourId,
                tour.Name,
                tour.Description,
                tour.Image,
                tour.Price,
                ProviderName = tour.Provider.Name,
                ProviderAvatar = tour.Provider.Avatar,
                ListImagesTours = tour.Listimagestours.Select(i => new
                {
                    i.ImageUrl,
                    i.ImageDescription
                }),
                Services = tour.Services.Select(s => new
                {
                    s.ServiceName,
                    s.Description,
                    Price = s.Price.HasValue ? s.Price.Value : 0
                }),
                Homestays = tour.Homestays.Select(h => new
                {
                    h.Name,
                    h.Address,
                    PricePerNight = h.PricePerNight.HasValue ? h.PricePerNight.Value : 0,
                    h.Description,
                    h.Image
                }),
                Reviews = tour.Reviews.Select(r => new
                {
                    CustomerName = r.Customer.Name,
                    r.Comment,
                    CreatedDate = r.Createdate
                })
            };

            return Ok(result); // Return the tour details as a JSON response
        }


        

    }
}
