using Microsoft.AspNetCore.Mvc;
using LuxuryLife.Areas.AdminQL.Controllers;

namespace LuxuryLife.Areas.AdminQL.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
