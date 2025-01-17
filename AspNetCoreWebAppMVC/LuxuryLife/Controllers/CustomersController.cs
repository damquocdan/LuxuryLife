using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using System.Text;

namespace LuxuryLife.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TourBookingContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public CustomersController(TourBookingContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: CustomerUser/Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: CustomerUser/Customers/Details/5
        public async Task<IActionResult> Details(int? customerId)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: CustomerUser/Customers/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: CustomerUser/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Name,Email,Password,Phone,Address,Dob,Demographics,Preferences,SearchHistory,CreateDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tạo HttpClient từ HttpClientFactory
                    var httpClient = _httpClientFactory.CreateClient();

                    // Đặt BaseAddress nếu cần
                    httpClient.BaseAddress = new Uri("https://api.example.com/"); // Thay bằng URL API thực tế

                    // Chuyển đổi đối tượng thành JSON
                    var jsonContent = JsonSerializer.Serialize(customer);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Gửi yêu cầu POST đến API
                    var response = await httpClient.PostAsync("api/Customers", httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        // Nếu API phản hồi thành công, chuyển hướng về trang Index
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Xử lý lỗi từ API
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"API Error: {errorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    // Ghi nhật ký và hiển thị thông báo lỗi nếu có ngoại lệ
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }

            // Trả về lại view với thông báo lỗi nếu có
            return View(customer);
        }

        // GET: CustomerUser/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerUser/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Email,Password,Phone,Address,Dob,Demographics,Preferences,SearchHistory,CreateDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Any() && files[0].Length > 0)
                    {
                        var file = files[0];

                        // Đảm bảo thư mục lưu trữ tồn tại
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/customers");
                        Directory.CreateDirectory(uploadsFolder);

                        // Tạo tên file duy nhất để tránh xung đột
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Lưu file vào thư mục
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn Avatar
                        customer.Avatar = "/images/customers/" + uniqueFileName;
                    }
                    else
                    {
                        // Nếu không upload file mới, giữ nguyên ảnh cũ
                        var existingAdmin = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(a => a.CustomerId == customer.CustomerId);
                        if (existingAdmin != null)
                        {
                            customer.Avatar = existingAdmin.Avatar;
                        }
                    }
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { CustomerId = customer.CustomerId });
            }
            return View(customer);
        }

        // GET: CustomerUser/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: CustomerUser/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
