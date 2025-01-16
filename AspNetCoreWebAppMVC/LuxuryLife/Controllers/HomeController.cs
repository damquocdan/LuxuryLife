
using LuxuryLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LuxuryLife.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory; // Inject HttpClientFactory
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = _httpClientFactory.CreateClient();
            string baseUrl = "http://localhost:7182/api/Home"; // Địa chỉ API của bạn

            // Fetch Latest Tours
            var toursResponse = await httpClient.GetAsync($"https://localhost:7182/api/Home/latest-tours");
            var tours = new List<Tour>();
            if (toursResponse.IsSuccessStatusCode)
            {
                var toursJson = await toursResponse.Content.ReadAsStringAsync();
                tours = JsonConvert.DeserializeObject<List<Tour>>(toursJson);
            }

            // Fetch Providers
            var providersResponse = await httpClient.GetAsync($"https://localhost:7182/api/Home/providers");
            var providers = new List<Provider>();
            if (providersResponse.IsSuccessStatusCode)
            {
                var providersJson = await providersResponse.Content.ReadAsStringAsync();
                providers = JsonConvert.DeserializeObject<List<Provider>>(providersJson);
            }

            // Fetch Latest News
            var newsResponse = await httpClient.GetAsync($"https://localhost:7182/api/Home/latest-news");
            var news = new List<News>();
            if (newsResponse.IsSuccessStatusCode)
            {
                var newsJson = await newsResponse.Content.ReadAsStringAsync();
                news = JsonConvert.DeserializeObject<List<News>>(newsJson);
            }

            // Fetch Customers
            var customersResponse = await httpClient.GetAsync($"https://localhost:7182/api/Home/customers");
            var customers = new List<Customer>();
            if (customersResponse.IsSuccessStatusCode)
            {
                var customersJson = await customersResponse.Content.ReadAsStringAsync();
                customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);
            }

            // Fetch Reviews
            var reviewsResponse = await httpClient.GetAsync($"https://localhost:7182/api/Home/reviews");
            var reviews = new List<Review>();
            if (reviewsResponse.IsSuccessStatusCode)
            {
                var reviewsJson = await reviewsResponse.Content.ReadAsStringAsync();
                reviews = JsonConvert.DeserializeObject<List<Review>>(reviewsJson);
            }

            // Pass data to the view
            ViewData["Tours"] = tours;
            ViewData["Providers"] = providers;
            ViewData["News"] = news;
            ViewData["Customers"] = customers;
            ViewData["Reviews"] = reviews;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}