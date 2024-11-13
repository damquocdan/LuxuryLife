using Microsoft.AspNetCore.Mvc;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
