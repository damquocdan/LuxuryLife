using Microsoft.AspNetCore.Mvc;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
