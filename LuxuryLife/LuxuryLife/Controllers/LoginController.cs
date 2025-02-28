using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LuxuryLife.Controllers
{
 
    public class LoginController : Controller
    {
        private readonly TourBookingContext _context;

        public LoginController(TourBookingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return View(model);
            }

            var dataLogin = _context.Customers
                .Where(x => x.Email == model.Email && x.Password == model.Password)
                .FirstOrDefault();

            if (dataLogin != null)
            {
                HttpContext.Session.SetString("CustomerLogin", model.Email);
                HttpContext.Session.SetInt32("CustomerId", dataLogin.CustomerId);

                return RedirectToAction("Index", "Home", new { customerId = dataLogin.CustomerId });
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
            HttpContext.Session.Remove("CustomerLogin");
            HttpContext.Session.Remove("CustomerId");

            return RedirectToAction("Index");
        }
    }
}
