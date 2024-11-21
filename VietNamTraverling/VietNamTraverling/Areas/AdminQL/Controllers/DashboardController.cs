using Microsoft.AspNetCore.Mvc;
using VietNamTraverling.Areas.AdminQL.Controllers;

namespace VietNamTraverling.Areas.AdminQL.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
