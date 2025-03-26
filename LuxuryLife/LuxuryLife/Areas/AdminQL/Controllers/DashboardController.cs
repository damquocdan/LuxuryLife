using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = new DashboardViewModel
            {
                // Pie Chart Data
                TourStatusDistribution = await GetTourStatusDistribution(),
                RatingDistribution = await GetRatingDistribution(),
                HomestayByProvider = await GetHomestayByProvider(),
                RevenueByHomestay = await GetRevenueByHomestay(),

                // Bar Chart Data
                ToursByProvider = await GetToursByProvider(),
                BookingsByMonth = await GetBookingsByMonth(),
                RevenueByProviderByMonth = await GetRevenueByProviderByMonth(), // Updated method
                ServicesByTour = await GetServicesByTour(),

                // Tổng hợp số liệu
                TotalTours = await _context.Tours.CountAsync(),
                TotalHomestays = await _context.Homestays.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalCustomers = await _context.Bookings.Select(b => b.CustomerId).Distinct().CountAsync(),
                TotalReviews = await _context.Reviews.CountAsync(),
                TotalContacts = await _context.Contacts.CountAsync(),

                // Doanh thu & số liệu khác
                TotalRevenue = await GetTotalRevenue(),
                TopReviewedTour = await GetTopReviewedTour(),
                TopProviderByTours = await GetTopProviderByTours(),
                TopCustomerByBookings = await GetTopCustomerByBookings()
            };

            return View(dashboardViewModel);
        }

        #region Pie Chart Methods
        private async Task<Dictionary<string, int>> GetTourStatusDistribution()
        {
            return await _context.Tours
                .GroupBy(t => t.Status ?? "Unknown")
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        private async Task<Dictionary<int, int>> GetRatingDistribution()
        {
            return await _context.Reviews
                .GroupBy(r => (int)r.Rating)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        private async Task<Dictionary<string, int>> GetHomestayByProvider()
        {
            return await _context.Homestays
                .Include(h => h.Tour.Provider) // Load dữ liệu từ bảng Provider
                .Where(h => h.ProviderId != null && h.Tour.Provider != null) // Kiểm tra Provider tồn tại
                .GroupBy(h => h.Tour.Provider.Name) // Nhóm theo tên nhà cung cấp
                .ToDictionaryAsync(g => g.Key, g => g.Count()); // Trả về Dictionary<string, int>
        }


        private async Task<Dictionary<int, decimal>> GetRevenueByHomestay()
        {
            return await _context.Bookings
                .Where(b => b.Status == "Completed" && b.TourId.HasValue)
                .Join(_context.Tours.Where(t => t.HomestayId.HasValue),
                    b => b.TourId,
                    t => t.TourId,
                    (b, t) => new { b.TotalPrice, HomestayId = t.HomestayId.Value })
                .GroupBy(x => x.HomestayId)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(x => x.TotalPrice ?? 0m));
        }
        #endregion

        #region Bar Chart Methods
        private async Task<Dictionary<int, int>> GetToursByProvider()
        {
            return await _context.Tours
                .Where(t => t.ProviderId.HasValue)
                .GroupBy(t => t.ProviderId.Value)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        private async Task<Dictionary<int, int>> GetBookingsByMonth()
        {
            return await _context.Bookings
                .Where(b => b.BookingDate.HasValue)
                .GroupBy(b => b.BookingDate.Value.Month)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        private async Task<Dictionary<string, decimal>> GetRevenueByProviderByMonth()
        {
            var revenueData = await _context.Bookings
                .Where(b => b.Status == "Confirmed" && b.TourId.HasValue && b.BookingDate.HasValue)
                .Join(_context.Tours.Where(t => t.ProviderId.HasValue),
                    b => b.TourId,
                    t => t.TourId,
                    (b, t) => new { b.TotalPrice, t.Provider.Name, b.BookingDate })
                .GroupBy(x => new { x.Name, Month = x.BookingDate.Value.Month })
                .Select(g => new
                {
                    Key = $"Provider {g.Key.Name} - Tháng {g.Key.Month}",
                    Revenue = g.Sum(x => x.TotalPrice ?? 0m)
                })
                .ToListAsync();

            return revenueData.ToDictionary(
                x => x.Key,
                x => x.Revenue
            );
        }

        private async Task<Dictionary<int, int>> GetServicesByTour()
        {
            return await _context.Services
                .Where(s => s.TourId.HasValue)
                .GroupBy(s => s.TourId.Value)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
        #endregion

        #region Revenue Methods
        private async Task<decimal> GetTotalRevenue()
        {
            return await _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .SumAsync(b => (b.TotalPrice ?? 0m) * 0.3m);
        }

        private async Task<(string TourName, int TourId, int ReviewCount)> GetTopReviewedTour()
        {
            var topTour = await _context.Reviews
                .Where(r => r.TourId.HasValue)
                .GroupBy(r => new { r.TourId, r.Tour.Name }) // Nhóm theo ID và tên tour
                .Select(g => new
                {
                    TourId = g.Key.TourId.Value, // ID của tour
                    TourName = g.Key.Name, // Tên của tour
                    Count = g.Count() // Số lượt đánh giá
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();

            return topTour != null ? (topTour.TourName, topTour.TourId, topTour.Count) : ("Không có dữ liệu", 0, 0);
        }


        private async Task<(string ProviderName, int ProviderId, int TourCount)> GetTopProviderByTours()
        {
            var topProvider = await _context.Tours
                .Where(t => t.ProviderId.HasValue)
                .GroupBy(t => new { t.ProviderId, t.Provider.Name }) // Nhóm theo ID và tên nhà cung cấp
                .Select(g => new
                {
                    ProviderId = g.Key.ProviderId.Value, // ID nhà cung cấp
                    ProviderName = g.Key.Name, // Tên nhà cung cấp
                    Count = g.Count() // Số tour cung cấp
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();

            return topProvider != null ? (topProvider.ProviderName, topProvider.ProviderId, topProvider.Count) : ("Không có dữ liệu", 0, 0);
        }


        private async Task<(string CustomerName, int CustomerId, int BookingCount)> GetTopCustomerByBookings()
        {
            var topCustomer = await _context.Bookings
                .Where(b => b.CustomerId.HasValue)
                .GroupBy(b => new { b.CustomerId, b.Customer.Name }) // Nhóm theo ID và tên khách hàng
                .Select(g => new
                {
                    CustomerId = g.Key.CustomerId.Value, // Lấy ID khách hàng
                    CustomerName = g.Key.Name, // Lấy tên khách hàng
                    Count = g.Count() // Số lần đặt
                })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();

            return topCustomer != null ? (topCustomer.CustomerName, topCustomer.CustomerId, topCustomer.Count) : ("Không có dữ liệu", 0, 0);
        }



        #endregion
    }

    public class DashboardViewModel
    {
        // Pie Chart Data
        public Dictionary<string, int> TourStatusDistribution { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; }
        public Dictionary<string, int> HomestayByProvider { get; set; }

        public Dictionary<int, decimal> RevenueByHomestay { get; set; }

        // Bar Chart Data
        public Dictionary<int, int> ToursByProvider { get; set; }
        public Dictionary<int, int> BookingsByMonth { get; set; }
        public Dictionary<string, decimal> RevenueByProviderByMonth { get; set; } // Changed to string key
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
        public (string TourName, int TourId, int ReviewCount) TopReviewedTour { get; set; }

        public (string ProviderName, int ProviderId, int TourCount) TopProviderByTours { get; set; }

        public (string CustomerName, int CustomerId, int BookingCount) TopCustomerByBookings { get; set; }
    }
}