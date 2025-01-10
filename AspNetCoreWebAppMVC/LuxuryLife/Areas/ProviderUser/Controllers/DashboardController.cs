using Microsoft.AspNetCore.Mvc;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
