using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using System.Linq;

namespace LuxuryLife.Areas.AdminQL.Controllers
{
    [Area("AdminQL")]
    public class DashboardController : BaseController
    {
        private readonly TourBookingContext _context;

        public DashboardController(TourBookingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                // 1. Pie Chart Data
                TourStatusDistribution = GetTourStatusDistribution(),
                RatingDistribution = GetRatingDistribution(),
                HomestayByProvider = GetHomestayByProvider(),
                RevenueByHomestay = GetRevenueByHomestay(),

                // 2. Bar Chart Data
                ToursByProvider = GetToursByProvider(),
                BookingsByMonth = GetBookingsByMonth(),
                RevenueByTour = GetRevenueByTour(),
                ServicesByTour = GetServicesByTour(),

                // 3. Tổng hợp số liệu
                TotalTours = _context.Tours.Count(),
                TotalHomestays = _context.Homestays.Count(),
                TotalBookings = _context.Bookings.Count(),
                TotalCustomers = _context.Bookings.Select(b => b.CustomerId).Distinct().Count(),
                TotalReviews = _context.Reviews.Count(),
                TotalContacts = _context.Contacts.Count(),

                // 4. Doanh thu & số liệu khác
                TotalRevenue = GetTotalRevenue(),
                TopReviewedTour = GetTopReviewedTour(),
                TopProviderByTours = GetTopProviderByTours(),
                TopCustomerByBookings = GetTopCustomerByBookings()
            };

            return View(dashboardViewModel);
        }

        #region Pie Chart Methods
        private Dictionary<string, int> GetTourStatusDistribution()
        {
            return _context.Tours
                .GroupBy(t => t.Status ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private Dictionary<int, int> GetRatingDistribution()
        {
            return _context.Reviews
                .GroupBy(r => (int)r.Rating) // Ép kiểu decimal sang int nếu Rating là decimal
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private Dictionary<int, int> GetHomestayByProvider()
        {
            return _context.Homestays
                .Where(h => h.ProviderId.HasValue) // Loại bỏ null
                .GroupBy(h => h.ProviderId.Value) // Sử dụng .Value
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private Dictionary<int, decimal> GetRevenueByHomestay()
        {
            return _context.Bookings
                .Where(b => b.Status == "Completed" && b.TourId.HasValue)
                .Join(_context.Tours.Where(t => t.HomestayId.HasValue),
                    b => b.TourId,
                    t => t.TourId,
                    (b, t) => new { b.TotalPrice, HomestayId = t.HomestayId.Value }) // Sử dụng .Value
                .GroupBy(x => x.HomestayId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalPrice ?? 0m));
        }
        #endregion

        #region Bar Chart Methods
        private Dictionary<int, int> GetToursByProvider()
        {
            return _context.Tours
                .Where(t => t.ProviderId.HasValue) // Loại bỏ null
                .GroupBy(t => t.ProviderId.Value) // Sử dụng .Value
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private Dictionary<int, int> GetBookingsByMonth()
        {
            return _context.Bookings
                .Where(b => b.BookingDate.HasValue) // Loại bỏ null
                .GroupBy(b => b.BookingDate.Value.Month) // Sử dụng .Value
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private Dictionary<int, decimal> GetRevenueByTour()
        {
            return _context.Bookings
                .Where(b => b.Status == "Completed" && b.TourId.HasValue) // Loại bỏ null
                .GroupBy(b => b.TourId.Value) // Sử dụng .Value
                .ToDictionary(g => g.Key, g => g.Sum(b => b.TotalPrice ?? 0m));
        }

        private Dictionary<int, int> GetServicesByTour()
        {
            return _context.Services
                .Where(s => s.TourId.HasValue) // Loại bỏ null
                .GroupBy(s => s.TourId.Value) // Sử dụng .Value
                .ToDictionary(g => g.Key, g => g.Count());
        }
        #endregion

        #region Revenue Methods
        private decimal GetTotalRevenue()
        {
            return _context.Bookings
                .Where(b => b.Status == "Completed")
                .Sum(b => b.TotalPrice ?? 0m);
        }

        private (int TourId, int ReviewCount) GetTopReviewedTour()
        {
            var topTour = _context.Reviews
                .Where(r => r.TourId.HasValue) // Loại bỏ null
                .GroupBy(r => r.TourId.Value) // Sử dụng .Value
                .Select(g => new { TourId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return topTour != null ? (topTour.TourId, topTour.Count) : (0, 0);
        }

        private (int ProviderId, int TourCount) GetTopProviderByTours()
        {
            var topProvider = _context.Tours
                .Where(t => t.ProviderId.HasValue) // Loại bỏ null
                .GroupBy(t => t.ProviderId.Value) // Sử dụng .Value
                .Select(g => new { ProviderId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return topProvider != null ? (topProvider.ProviderId, topProvider.Count) : (0, 0);
        }

        private (int CustomerId, int BookingCount) GetTopCustomerByBookings()
        {
            var topCustomer = _context.Bookings
                .Where(b => b.CustomerId.HasValue) // Loại bỏ null
                .GroupBy(b => b.CustomerId.Value) // Sử dụng .Value
                .Select(g => new { CustomerId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            return topCustomer != null ? (topCustomer.CustomerId, topCustomer.Count) : (0, 0);
        }
        #endregion
    }

    public class DashboardViewModel
    {
        // Pie Chart Data
        public Dictionary<string, int> TourStatusDistribution { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; }
        public Dictionary<int, int> HomestayByProvider { get; set; }
        public Dictionary<int, decimal> RevenueByHomestay { get; set; }

        // Bar Chart Data
        public Dictionary<int, int> ToursByProvider { get; set; }
        public Dictionary<int, int> BookingsByMonth { get; set; }
        public Dictionary<int, decimal> RevenueByTour { get; set; }
        public Dictionary<int, int> ServicesByTour { get; set; }

        // Tổng hợp số liệu
        public int TotalTours { get; set; }
        public int TotalHomestays { get; set; }
        public int TotalBookings { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalReviews { get; set; }
        public int TotalContacts { get; set; }

        // Doanh thu & số liệu khác
        public decimal TotalRevenue { get; set; }
        public (int TourId, int ReviewCount) TopReviewedTour { get; set; }
        public (int ProviderId, int TourCount) TopProviderByTours { get; set; }
        public (int CustomerId, int BookingCount) TopCustomerByBookings { get; set; }
    }
}