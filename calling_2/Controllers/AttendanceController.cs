using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Calling_2.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AttendanceController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:7287/api/") // Your API base URL
            };
        }

        // Mark Attendance API (POST /api/attendance/mark)
        [HttpPost]
        public async Task<ActionResult> MarkAttendance(int userId, bool status)
        {
            var payload = new { UserId = userId, Status = status };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("attendance/mark", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Attendance marked successfully!";
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to mark attendance.";
            }
            return View();
        }

        // Get Attendance for a User API (GET /api/attendance/{userId})
        [HttpGet]
        public async Task<ActionResult> GetAttendance(int userId)
        {
            var response = await _httpClient.GetAsync($"attendance/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                ViewBag.AttendanceRecords = JsonConvert.DeserializeObject(result);
            }
            return View();
        }

        // Get Attendance for a Class API (GET /api/attendance/class/{classId})
        [HttpGet]
        public async Task<ActionResult> GetClassAttendance(int classId)
        {
            var response = await _httpClient.GetAsync($"attendance/class/{classId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                ViewBag.ClassAttendance = JsonConvert.DeserializeObject(result);
            }
            return View();
        }
    }
}
