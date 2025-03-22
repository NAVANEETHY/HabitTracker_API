using HabitTracker_API.Interfaces;
using HabitTracker_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HabitTracker_API.Controllers
{
    [Route("habittracker/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOtpService iOtpService;
        private readonly ISPService iSpService;
        public UserController(IOtpService iOtpService, ISPService iSpService)
        {
            this.iOtpService = iOtpService;
            this.iSpService = iSpService;
        }

        [HttpPost("sendotp")]
        public async Task<IActionResult> SendOtp(dynamic json)
        {
            try
            {
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                string toEmail = jsonMap["email"].ToString();
                string otp = iOtpService.GenerateOtp();
                await iOtpService.SendOtpViaEmail(toEmail, otp);
                return Content("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> InsertUser(dynamic json)
        {
            string jsonStr;
            try
            {
                jsonStr = JsonSerializer.Serialize(json);
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                string toEmail = jsonMap["email"].ToString();
                string otp = jsonMap["otp"].ToString();
                if(!iOtpService.ValidateOtp(toEmail, otp))
                {
                    throw new Exception("Invalid otp");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return await iSpService.ExecuteSP("spInsertUser", jsonStr);
        }
    }
}
