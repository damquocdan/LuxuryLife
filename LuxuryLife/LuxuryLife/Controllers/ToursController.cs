using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ToursController : Controller
    {
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string query, string description)
        {
            // Lấy CustomerId từ Session
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

                    // Kết hợp các từ khóa
                    string newSearch = string.Join(" ", searchKeywords);

                    if (!string.IsNullOrEmpty(newSearch))
                    {
                        // Lấy lịch sử tìm kiếm hiện tại
                        var currentHistory = string.IsNullOrEmpty(customer.SearchHistory)
                            ? new List<string>()
                            : customer.SearchHistory.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                        // Thêm tìm kiếm mới vào lịch sử
                        if (!currentHistory.Contains(newSearch))
                        {
                            // Giới hạn số lượng lịch sử tìm kiếm (ví dụ: 10 mục)
                            const int maxHistoryItems = 10;

                            currentHistory.Insert(0, newSearch); // Thêm vào đầu danh sách
                            if (currentHistory.Count > maxHistoryItems)
                            {
                                currentHistory = currentHistory.Take(maxHistoryItems).ToList();
                            }

                            // Cập nhật SearchHistory
                            customer.SearchHistory = string.Join(" ", currentHistory);
                            _context.Update(customer);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            // Lọc theo query (tên hoặc mô tả)
            if (!string.IsNullOrEmpty(query))
            {
                tours = tours.Where(t => t.Name.ToLower().Contains(query.ToLower()) ||
                                       t.Description.ToLower().Contains(query.ToLower()));
            }

            // Lọc theo description riêng nếu có
            if (!string.IsNullOrEmpty(description))
            {
                tours = tours.Where(t => t.Description.ToLower().Contains(description.ToLower()));
            }
            List<Tour> finalTours = new List<Tour>();
            List<Tour> recommendedTours = new List<Tour>();
            var suggestedTourIds = new HashSet<int>();

            if (customerId.HasValue)
            {
                // **1. Lấy danh sách tour yêu thích (Priority 1)**
                var favoriteToursQuery = tours
                    .Where(t => _context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId))
                    .Select(t => new { Tour = t, Priority = 1, MatchScore = 100 });

                // **2. Gợi ý dựa trên sở thích, lịch sử tìm kiếm (Priority 2)**
                var customerInfo = await _context.Customers
                    .Where(c => c.CustomerId == customerId)
                    .Select(c => new
                    {
                        c.Preferences,
                        c.SearchHistory,
                        c.Demographics
                    })
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
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId)) // Loại bỏ favorite tours
                    .Select(t => new
                    {
                        Tour = t,
                        MatchScore = customerKeywords.Any() ?
                            customerKeywords.Count(k => t.Description.ToLower().Contains(k.ToLower())) : 0
                    })
                    .Where(t => t.MatchScore > 0)
                    .OrderByDescending(t => t.MatchScore)
                    .Select(t => new { t.Tour, Priority = 2, t.MatchScore });

                // **3. Gợi ý dựa trên tour đã đặt và đánh giá (Priority 3)**
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
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId)) // Loại bỏ favorite tours
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
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId)) // Loại bỏ favorite tours
                    .Where(t => allRecommendedTourIds.Contains(t.TourId))
                    .Select(t => new { Tour = t, Priority = 3, MatchScore = 50 });

                // **4. Các tour còn lại (Priority 4)**
                var remainingToursQuery = tours
                    .Where(t => !_context.Favourites.Any(f => f.CustomerId == customerId && f.TourId == t.TourId)) // Loại bỏ favorite tours
                    .Where(t => !allRecommendedTourIds.Contains(t.TourId)) // Loại bỏ recommended tours
                    .Select(t => new { Tour = t, Priority = 4, MatchScore = 0 });

                // **Kết hợp tất cả danh sách tour theo thứ tự ưu tiên**
                var allToursQuery = favoriteToursQuery
                    .Union(recommendedByPreferencesQuery) // Sử dụng Union thay vì Concat để giữ IQueryable
                    .Union(recommendedByBookingsQuery)
                    .Union(remainingToursQuery)
                    .OrderBy(t => t.Priority)
                    .ThenByDescending(t => t.MatchScore)
                    .ThenByDescending(t => t.Tour.Createdate)
                    .Select(t => t.Tour);

                finalTours = await allToursQuery.ToListAsync();

                // **Lưu danh sách recommended tours riêng (Priority 2 và 3)**
                recommendedTours = await recommendedByPreferencesQuery
                    .Union(recommendedByBookingsQuery)
                    .Select(t => t.Tour)
                    .ToListAsync();
            }
            else
            {
                // **Nếu chưa đăng nhập, hiển thị danh sách mặc định theo điều kiện lọc**
                finalTours = await tours
                    .OrderByDescending(t => t.Createdate)
                    .ToListAsync();
            }

            // Load dữ liệu vào ViewData
            ViewData["Tours"] = finalTours;
            ViewData["RecommendedTours"] = recommendedTours;
            ViewData["Providers"] = await _context.Providers.ToListAsync();
            ViewData["Customers"] = await _context.Customers.ToListAsync();
            ViewData["Reviews"] = await _context.Reviews
                .Include(r => r.Tour)
                .Include(r => r.Customer)
                .ToListAsync();

            return View(finalTours);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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

            // Load CurrentUser (có thể null nếu chưa đăng nhập)
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            ViewBag.CurrentUser = customerId.HasValue
                ? await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId.Value)
                : null;

            ViewBag.RelatedTours = relatedTours;

            return View(tour);
        }
        private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.TourId == id);
        }
    }
}
