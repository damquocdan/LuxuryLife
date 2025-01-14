using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using System.Net.Http;
using static System.Reflection.Metadata.BlobBuilder;

namespace LuxuryLife.Controllers
{
    public class ToursController : Controller
    {
        private readonly HttpClient _httpClient;
        public ToursController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            // Call the API to fetch tours
            IEnumerable<Tour> tours = new List<Tour>();

            try
            {
                tours = await _httpClient.GetFromJsonAsync<IEnumerable<Tour>>("https://localhost:7182/api/Tours/Index");
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                Console.WriteLine($"Error fetching tours: {ex.Message}");
            }

            // Pass the tours to the view
            return View(tours);
        }


        // GET: Tours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Call the API to get the tour details
            var tour = await _httpClient.GetFromJsonAsync<Tour>($"https://localhost:7182/api/Tours/{id}");

            if (tour == null)
            {
                return NotFound();
            }

            // Return the view with the tour details
            return View(tour);
        }

        // GET: Tours/Search
        public async Task<IActionResult> Search(string query)
        {
            List<Tour> tours;
            if (string.IsNullOrEmpty(query))
            {
                // Nếu từ khóa tìm kiếm rỗng, lấy tất cả sách
                tours = await _httpClient.GetFromJsonAsync<List<Tour>>("tours");
            }
            else
            {
                // Nếu có từ khóa tìm kiếm, gọi API tìm kiếm
                tours = await _httpClient.GetFromJsonAsync<List<Tour>>($"tours/search?query={query}");
            }

            return View("Index", tours); // Trả về view Index với danh sách sách tìm được
        }

    }
}
