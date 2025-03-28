using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ContactsController : Controller
    {
        private readonly TourBookingContext _context;

        public ContactsController(TourBookingContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var tourBookingContext = _context.Contacts.Include(c => c.Customer);
            return View(await tourBookingContext.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", customerId);
            // Nếu khách hàng đã đăng nhập, truyền CustomerId vào ViewBag để sử dụng trong view
            ViewBag.LoggedInCustomerId = customerId;
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,CustomerId,Name,Email,Phone,Subject,Message,CreatedDate,Status")] Contact contact)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId.HasValue)
            {
                // Nếu khách hàng đã đăng nhập, gán CustomerId từ session
                contact.CustomerId = customerId.Value;
            }
            else
            {
                // Nếu chưa đăng nhập, để CustomerId là null
                contact.CustomerId = null;
            }

            // Gán CreatedDate nếu chưa có
            if (contact.CreatedDate == default)
            {
                contact.CreatedDate = DateTime.UtcNow;
            }

            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Liên hệ của bạn đã được gửi thành công! Chúng tôi sẽ phản hồi sớm nhất có thể.";
                return RedirectToAction(nameof(Create));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Email", contact.CustomerId);
            ViewBag.LoggedInCustomerId = customerId;
            return View(contact);
        }

        // GET: Contacts/Edit/5

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
