using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuxuryLife.Models;
using Newtonsoft.Json;

namespace LuxuryLife.Controllers
{
    public class ToursController : Controller
    {
        private readonly HttpClient _httpClient;

        public ToursController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {

                return View();
        }


        public async Task<IActionResult> Search(string query, string description)
        {
            List<Tour> tours;

            // Build the search query string dynamically
            var searchUrl = "https://localhost:7182/api/Tours";

            // If query or description is provided, add them to the URL
            if (!string.IsNullOrEmpty(query) || !string.IsNullOrEmpty(description))
            {
                // Append both query and description as search parameters
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(query))
                {
                    queryParams.Add($"query={query}");
                }

                if (!string.IsNullOrEmpty(description))
                {
                    queryParams.Add($"description={description}");
                }

                // Combine all query parameters
                searchUrl = $"{searchUrl}/search?{string.Join("&", queryParams)}";
            }

            // Fetch the tour data from the API
            tours = await _httpClient.GetFromJsonAsync<List<Tour>>(searchUrl);

            // Return the tours to the Index view
            return View("Index", tours);
        }



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
    }
}