using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LuxuryLife.Areas.ProviderUser.Controllers
{
    [Area("ProviderUser")]
    public class LoginController : Controller
    {
        private readonly LuxuryLifeContext _context;

        public LoginController(LuxuryLifeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginProvider model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return View(model);
            }

            var dataLogin = _context.Providers
                .Where(x => x.Email == model.Email && x.Password == model.Password)
                .FirstOrDefault();

            if (dataLogin != null)
            {
                HttpContext.Session.SetString("ProviderLogin", model.Email);
                HttpContext.Session.SetInt32("ProviderId", dataLogin.ProviderId);

                return RedirectToAction("Index", "Dashboard", new {providerId = dataLogin.ProviderId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ProviderLogin");
            HttpContext.Session.Remove("ProviderId");

            return RedirectToAction("Index");
        }
    }
}
