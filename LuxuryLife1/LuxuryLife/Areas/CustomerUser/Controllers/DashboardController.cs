﻿using LuxuryLife.Controllers;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly LuxuryLifeContext _context; // Thêm DbContext

        public DashboardController(ILogger<DashboardController> logger, LuxuryLifeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var tours = _context.Tours.Include(t => t.Provider).ToList();
            return View(tours);
        }
    }
}
