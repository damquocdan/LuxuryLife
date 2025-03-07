using Microsoft.AspNetCore.Mvc;
using LuxuryLife.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    [Area("ProviderUser")]
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
                TourBookingsByTime = GetTourBookingsByTime(),
                TourRevenueByTime = GetTourRevenueByTime(),
                MostBookedTours = GetMostBookedTours(),

                HomestayBookingsByTime = GetHomestayBookingsByTime(),
                MostBookedHomestays = GetMostBookedHomestays(),
                HomestayRevenue = GetHomestayRevenue(),

                TotalCustomers = GetTotalCustomers(),
                TopTourBookingCustomers = GetTopTourBookingCustomers(),
                TopHomestayBookingCustomers = GetTopHomestayBookingCustomers(),

                TotalTourRevenue = GetTotalTourRevenue(),
                TotalHomestayRevenue = GetTotalHomestayRevenue(),
                MonthlyRevenueChart = GetMonthlyRevenueChart()
            };

            return View(dashboardViewModel);
        }

        #region Tour Statistics
        private Dictionary<string, int> GetTourBookingsByTime()
        {
            var today = DateTime.Today;
            return new Dictionary<string, int>
            {
                ["Daily"] = _context.Bookings.Count(b => b.BookingDate.HasValue && b.BookingDate.Value == today),
                ["Monthly"] = _context.Bookings.Count(b => b.BookingDate.HasValue && b.BookingDate.Value.Month == today.Month && b.BookingDate.Value.Year == today.Year),
                ["Yearly"] = _context.Bookings.Count(b => b.BookingDate.HasValue && b.BookingDate.Value.Year == today.Year)
            };
        }

        private Dictionary<string, decimal> GetTourRevenueByTime()
        {
            var today = DateTime.Today;
            return new Dictionary<string, decimal>
            {
                ["Daily"] = _context.Bookings.Where(b => b.BookingDate.HasValue && b.BookingDate.Value == today).Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0),
                ["Monthly"] = _context.Bookings.Where(b => b.BookingDate.HasValue && b.BookingDate.Value.Month == today.Month && b.BookingDate.Value.Year == today.Year).Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0),
                ["Yearly"] = _context.Bookings.Where(b => b.BookingDate.HasValue && b.BookingDate.Value.Year == today.Year).Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0)
            };
        }

        private List<TourBookingStats> GetMostBookedTours()
        {
            return _context.Bookings
                .GroupBy(b => b.Tour)
                .Select(g => new TourBookingStats
                {
                    Tour = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToList();
        }
        #endregion

        #region Homestay Statistics
        private Dictionary<string, int> GetHomestayBookingsByTime()
        {
            var today = DateTime.Today;
            return new Dictionary<string, int>
            {
                ["Daily"] = _context.Bookings.Count(b => b.Tour != null && b.Tour.Homestays.Any() && b.BookingDate.HasValue && b.BookingDate.Value == today),
                ["Monthly"] = _context.Bookings.Count(b => b.Tour != null && b.Tour.Homestays.Any() && b.BookingDate.HasValue && b.BookingDate.Value.Month == today.Month && b.BookingDate.Value.Year == today.Year),
                ["Yearly"] = _context.Bookings.Count(b => b.Tour != null && b.Tour.Homestays.Any() && b.BookingDate.HasValue && b.BookingDate.Value.Year == today.Year)
            };
        }

        private List<HomestayBookingStats> GetMostBookedHomestays()
        {
            return _context.Homestays
                .Select(h => new HomestayBookingStats
                {
                    Homestay = h,
                    BookingCount = _context.Bookings.Count(b => b.TourId.HasValue && b.TourId == h.TourId)
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToList();
        }

        private decimal GetHomestayRevenue()
        {
            return _context.Bookings
                .Where(b => b.Tour != null && b.Tour.Homestays.Any())
                .Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0);
        }
        #endregion

        #region Customer Statistics
        private int GetTotalCustomers()
        {
            return _context.Customers.Count();
        }

        private List<CustomerBookingStats> GetTopTourBookingCustomers()
        {
            return _context.Customers
                .Select(c => new CustomerBookingStats
                {
                    Customer = c,
                    BookingCount = c.Bookings.Count(b => b.TourId.HasValue)
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToList();
        }

        private List<CustomerBookingStats> GetTopHomestayBookingCustomers()
        {
            return _context.Customers
                .Select(c => new CustomerBookingStats
                {
                    Customer = c,
                    BookingCount = c.Bookings.Count(b => b.Tour != null && b.Tour.Homestays.Any())
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToList();
        }
        #endregion

        #region Revenue Statistics
        private decimal GetTotalTourRevenue()
        {
            return _context.Bookings.Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0);
        }

        private decimal GetTotalHomestayRevenue()
        {
            return _context.Bookings
                .Where(b => b.Tour != null && b.Tour.Homestays.Any())
                .Sum(b => b.TotalPrice.HasValue ? b.TotalPrice.Value : 0);
        }

        private Dictionary<string, decimal> GetMonthlyRevenueChart()
        {
            var revenueData = _context.Bookings
                .Where(b => b.BookingDate.HasValue)
                .GroupBy(b => new { b.BookingDate.Value.Year, b.BookingDate.Value.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(e => e.TotalPrice.HasValue ? e.TotalPrice.Value : 0)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList(); // Fetch data to client

            return revenueData.ToDictionary(
                x => $"{x.Month}/{x.Year}", // Format client-side
                x => x.Revenue
            );
        }
        #endregion
    }

    public class DashboardViewModel
    {
        public Dictionary<string, int> TourBookingsByTime { get; set; }
        public Dictionary<string, decimal> TourRevenueByTime { get; set; }
        public List<TourBookingStats> MostBookedTours { get; set; }
        public Dictionary<string, int> HomestayBookingsByTime { get; set; }
        public List<HomestayBookingStats> MostBookedHomestays { get; set; }
        public decimal HomestayRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public List<CustomerBookingStats> TopTourBookingCustomers { get; set; }
        public List<CustomerBookingStats> TopHomestayBookingCustomers { get; set; }
        public decimal TotalTourRevenue { get; set; }
        public decimal TotalHomestayRevenue { get; set; }
        public Dictionary<string, decimal> MonthlyRevenueChart { get; set; }
    }

    public class TourBookingStats
    {
        public Tour Tour { get; set; }
        public int BookingCount { get; set; }
    }

    public class HomestayBookingStats
    {
        public Homestay Homestay { get; set; }
        public int BookingCount { get; set; }
    }

    public class CustomerBookingStats
    {
        public Customer Customer { get; set; }
        public int BookingCount { get; set; }
    }
}