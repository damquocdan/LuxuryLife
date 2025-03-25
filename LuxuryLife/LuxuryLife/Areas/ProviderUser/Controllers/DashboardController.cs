using Microsoft.AspNetCore.Mvc;
using LuxuryLife.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

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
            return View();
        }

        [HttpGet]
        public IActionResult GetDashboardData(int providerId, int year)
        {
            // Doanh thu theo tháng
            var revenueData = _context.Bookings
                .Where(b => b.Tour.ProviderId == providerId && b.BookingDate.HasValue && b.BookingDate.Value.Year == year)
                .GroupBy(b => b.BookingDate.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Revenue = g.Sum(b => b.TotalPrice.GetValueOrDefault()) * 0.7m
                })
                .OrderBy(g => g.Month)
                .ToList();

            // Số lượng đặt chỗ theo tháng
            var bookingData = _context.Bookings
                .Where(b => b.Tour.ProviderId == providerId && b.BookingDate.HasValue && b.BookingDate.Value.Year == year)
                .GroupBy(b => b.BookingDate.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToList();

            // Tổng số tour
            var totalTours = _context.Tours
                .Count(t => t.ProviderId == providerId);

            // Tổng số đặt chỗ
            var totalBookings = _context.Bookings
                .Count(b => b.Tour.ProviderId == providerId && b.BookingDate.HasValue && b.BookingDate.Value.Year == year);

            // Đánh giá trung bình
            var averageRating = _context.Reviews
                .Where(r => r.Tour.ProviderId == providerId)
                .Average(r => (double?)r.Rating) ?? 0;

            var result = new
            {
                revenueData,
                bookingData,
                totalTours,
                totalBookings,
                averageRating
            };

            return Json(result);
        }
    }
}