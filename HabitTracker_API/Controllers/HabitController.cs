using HabitTracker_API.EFContexts;
using HabitTracker_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace HabitTracker_API.Controllers
{
    [Authorize(AuthenticationSchemes = "User_Authorization")]
    [Route("habittracker/api/[controller]")]
    [ApiController]
    public class HabitController : ControllerBase
    {
        private readonly ISPService iSpService;

        public HabitController(ISPService iSpService)
        {
            this.iSpService = iSpService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertHabit([FromBody] dynamic json)
        {
            string jsonStr = "";
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"), CultureInfo.InvariantCulture);
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                jsonMap.Add("UserID", userId);
                jsonStr = JsonSerializer.Serialize(jsonMap);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return await iSpService.ExecuteSP("spInsertHabit", jsonStr);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetHabitsEveryDay()
        {
            string jsonStr = "";
            try
            {
                int userId = int.Parse(User.FindFirstValue("userId"), CultureInfo.InvariantCulture);
                jsonStr += "{\"UserID\":" + userId + "}";
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return await iSpService.ExecuteSP("spGetHabitsEveryDay", jsonStr);
        }
    }
}
