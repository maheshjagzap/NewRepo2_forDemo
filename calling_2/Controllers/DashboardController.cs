using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Calling_2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;

        public DashboardController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:7287/api/") // Replace with your actual API URL
            };
        }

        // Admin Dashboard API
        [HttpGet]
        public async Task<ActionResult> AdminDashboard()
        {
            var response = await _httpClient.GetAsync("dashboard/admin");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var adminData = JsonConvert.DeserializeObject(result);
                ViewBag.AdminData = adminData;
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to retrieve admin data.";
            }
            return View();
        }
    }
}
