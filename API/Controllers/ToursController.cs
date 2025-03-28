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
    public class ToursController : ControllerBase
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tour>>> GetTours(string query = null, string description = null)
        {
            // Lấy CustomerId từ Session (hoặc từ token trong API context nếu dùng authentication)
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Lấy danh sách tour active
            var tours = _context.Tours
                .Where(t => t.Status == "Active")
                .Include(t => t.Provider)
                .AsQueryable();

            if (customerId.HasValue && (!string.IsNullOrEmpty(query) || !string.IsNullOrEmpty(description)))
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId.Value);

                if (customer != null)
                {
                    // Chuẩn bị từ khóa tìm kiếm
                    var searchKeywords = new List<string>();
                    if (!string.IsNullOrEmpty(query))
                        searchKeywords.Add(query.Trim().ToLower());
                    if (!string.IsNullOrEmpty(description))
                        searchKeywords.Add(description.Trim().ToLower());

                    string newSearch = string.Join(" ", searchKeywords);

                    if (!string.IsNullOrEmpty(newSearch))
                    {
                        var currentHistory = string.IsNullOrEmpty(customer.SearchHistory)
                            ? new List<string>()
                            : customer.SearchHistory.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (!currentHistory.Contains(newSearch))
                        {
                            const int maxHistoryItems = 10;
                            currentHistory.Insert(0, newSearch);
                            if (currentHistory.Count > maxHistoryItems)
                            {
                                currentHistory = currentHistory.Take(maxHistoryItems).ToList();
                            }
                            customer.SearchHistory = string.Join(" ", currentHistory);
                            _context.Update(customer);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            // Lọc theo query và description
            if (!string.IsNullOrEmpty(query))
            {
                tours = tours.Where(t => t.Name.ToLower().Contains(query.ToLower()) ||
                                       t.Description.ToLower().Contains(query.ToLower()));
            }

            if (!string.IsNullOrEmpty(description))
            {
                tours = tours.Where(t => t.Description.ToLower().Contains(description.ToLower()));
            }

            List<Tour> finalTours = new List<Tour>();
            List<Tour> recommendedTours = new List<Tour>();

            if (customerId.HasValue)
            {
                // Logic gợi ý tour (giống Index cũ)
                var favoriteToursQuery = tours
                    .Where(t => _context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Select(t => new { Tour = t, Priority = 1, MatchScore = 100 });

                var customerInfo = await _context.Customers
                    .Where(c => c.CustomerId == customerId)
                    .Select(c => new { c.Preferences, c.SearchHistory, c.Demographics })
                    .FirstOrDefaultAsync();

                var customerKeywords = new List<string>();
                if (customerInfo != null)
                {
                    if (!string.IsNullOrEmpty(customerInfo.Preferences))
                        customerKeywords.AddRange(customerInfo.Preferences.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    if (!string.IsNullOrEmpty(customerInfo.SearchHistory))
                        customerKeywords.AddRange(customerInfo.SearchHistory.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                    if (!string.IsNullOrEmpty(customerInfo.Demographics))
                        customerKeywords.AddRange(customerInfo.Demographics.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }

                var recommendedByPreferencesQuery = tours
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Select(t => new
                    {
                        Tour = t,
                        MatchScore = customerKeywords.Any() ?
                            customerKeywords.Count(k => t.Description.ToLower().Contains(k.ToLower())) : 0
                    })
                    .Where(t => t.MatchScore > 0)
                    .OrderByDescending(t => t.MatchScore)
                    .Select(t => new { t.Tour, Priority = 2, t.MatchScore });

                var bookedTourIds = await _context.Bookings
                    .Where(b => b.CustomerId == customerId)
                    .Select(b => b.TourId)
                    .Distinct()
                    .ToListAsync();

                var reviewedTourIds = await _context.Reviews
                    .Where(r => r.CustomerId == customerId)
                    .Select(r => r.TourId)
                    .Distinct()
                    .ToListAsync();

                var similarCustomers = await _context.Reviews
                    .Where(r => bookedTourIds.Contains(r.TourId) || reviewedTourIds.Contains(r.TourId))
                    .Select(r => r.CustomerId)
                    .Distinct()
                    .ToListAsync();

                var similarReviews = await _context.Reviews
                    .Where(r => similarCustomers.Contains(r.CustomerId) && r.Rating >= 4)
                    .Select(r => (int?)r.TourId)
                    .Distinct()
                    .ToListAsync();

                var bookedServices = await _context.Services
                    .Where(s => bookedTourIds.Contains(s.TourId))
                    .Select(s => s.ServiceId)
                    .Distinct()
                    .ToListAsync();

                var bookedHomestays = await _context.Homestays
                    .Where(h => bookedTourIds.Contains(h.TourId))
                    .Select(h => h.HomestayId)
                    .Distinct()
                    .ToListAsync();

                var contentBasedTourIds = await tours
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Where(t => _context.Services.Any(s => s.TourId == t.TourId && bookedServices.Contains(s.ServiceId)) ||
                               _context.Homestays.Any(h => h.TourId == t.TourId && bookedHomestays.Contains(h.HomestayId)))
                    .Select(t => (int?)t.TourId)
                    .Distinct()
                    .ToListAsync();

                var allRecommendedTourIds = similarReviews
                    .Where(id => id.HasValue).Select(id => id.Value)
                    .Concat(contentBasedTourIds.Where(id => id.HasValue).Select(id => id.Value))
                    .Distinct()
                    .ToList();

                var recommendedByBookingsQuery = tours
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Where(t => allRecommendedTourIds.Contains(t.TourId))
                    .Select(t => new { Tour = t, Priority = 3, MatchScore = 50 });

                var remainingToursQuery = tours
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Where(t => !allRecommendedTourIds.Contains(t.TourId))
                    .Select(t => new { Tour = t, Priority = 4, MatchScore = 0 });

                var allToursQuery = favoriteToursQuery
                    .Union(recommendedByPreferencesQuery)
                    .Union(recommendedByBookingsQuery)
                    .Union(remainingToursQuery)
                    .OrderBy(t => t.Priority)
                    .ThenByDescending(t => t.MatchScore)
                    .ThenByDescending(t => t.Tour.Createdate)
                    .Select(t => t.Tour);

                finalTours = await allToursQuery.ToListAsync();

                recommendedTours = await recommendedByPreferencesQuery
                    .Union(recommendedByBookingsQuery)
                    .Select(t => t.Tour)
                    .ToListAsync();
            }
            else
            {
                finalTours = await tours
                    .OrderByDescending(t => t.Createdate)
                    .ToListAsync();
            }

            // Trả về danh sách tour kèm recommended tours trong response
            return Ok(new
            {
                Tours = finalTours,
                RecommendedTours = recommendedTours
            });
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tour>> GetTour(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Provider)
                .Include(t => t.Listimagestours)
                .Include(t => t.Services)
                .Include(t => t.Homestays)
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.Customer)
                .Include(t => t.Reviews)
                    .ThenInclude(r => r.ReviewOns)
                    .ThenInclude(ro => ro.Customer)
                .FirstOrDefaultAsync(m => m.TourId == id);

            if (tour == null)
            {
                return NotFound();
            }

            var relatedTours = await _context.Tours
                .Where(t => t.TourId != id && t.Status == "Active")
                .OrderByDescending(t => t.Createdate)
                .ToListAsync();

            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var currentUser = customerId.HasValue
                ? await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId.Value)
                : null;

            return Ok(new
            {
                Tour = tour,
                RelatedTours = relatedTours,
                CurrentUser = currentUser
            });
        }

        // PUT: api/Tours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTour(int id, Tour tour)
        {
            if (id != tour.TourId)
            {
                return BadRequest();
            }

            _context.Entry(tour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TourExists(id))
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

        // POST: api/Tours
        [HttpPost]
        public async Task<ActionResult<Tour>> PostTour(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTour), new { id = tour.TourId }, tour);
        }

        // DELETE: api/Tours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                return NotFound();
            }

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}