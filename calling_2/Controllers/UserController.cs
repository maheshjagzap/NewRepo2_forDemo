using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Added this for session methods
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://yourapiurl.com/api/")
            };
        }

        // Login API
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("user/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<dynamic>(result).Token;

                // Store the JWT token in the session
                HttpContext.Session.SetString("JWTToken", token); // Make sure 'SetString' is correctly recognized

                return RedirectToAction("Dashboard", "Dashboard");
            }

            ViewBag.ErrorMessage = "Invalid credentials.";
            return View();
        }

        // Register API
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password, string role)
        {
            var payload = new { Name = name, Email = email, Password = password, Role = role };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("user/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ViewBag.ErrorMessage = "Registration failed.";
            return View();
        }

        // Get User Details API
        [HttpGet]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var response = await _httpClient.GetAsync($"user/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                ViewBag.UserDetails = JsonConvert.DeserializeObject(result);
            }
            return View();
        }
    }
}
