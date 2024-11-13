using LuxuryLife.Areas.CustomerUser.Models;
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryLife.Areas.CustomerUser.Controllers
{
    [Area("CustomerUser")]
    public class LoginController : Controller
    {
        public LuxuryLifeContext _context;
        public LoginController(LuxuryLifeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost] // POST -> khi submit form
        public IActionResult Index(LoginUser model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
                return View(model);
            }

            var pass = model.Password;
            var dataLogin = _context.Customers.FirstOrDefault(x => x.Email.Equals(model.Email) && x.Password.Equals(pass));
            if (dataLogin != null)
            {
                ViewBag.IsLoggedIn = true;
                HttpContext.Session.SetString("CustomerLogin", model.Email);
                HttpContext.Session.SetInt32("CustomerId", dataLogin.CustomerId); // Access StudentId from dataLogin
                return RedirectToAction("Index", "Dashboard", new { customerId = dataLogin.CustomerId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không chính xác.");
                return View(model);
            }

        }
        [HttpGet]// thoát đăng nhập, huỷ session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CustomerLogin"); // huỷ session với key AdminLogin đã lưu trước đó

            return RedirectToAction("Index");
        }
    }
}
