using HabitTracker_API.EFContexts;
using HabitTracker_API.Interfaces;
using HabitTracker_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace HabitTracker_API.Controllers
{
    [Route("habittracker/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOtpService iOtpService;
        private readonly ISPService iSpService;
        private readonly HabitDBContext habitDBContext;
        private readonly IJwtService jwtService;
        public UserController(
            IOtpService iOtpService, ISPService iSpService, 
            HabitDBContext habitDBContext, IJwtService jwtService
            )
        {
            this.iOtpService = iOtpService;
            this.iSpService = iSpService;
            this.habitDBContext = habitDBContext;
            this.jwtService = jwtService;
        }

        [HttpPost("sendotp")]
        public async Task<IActionResult> SendOtp([FromBody] dynamic json)
        {
            try
            {
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                string toEmail = jsonMap["Email"].ToString();
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
        public async Task<IActionResult> UserSignUp([FromBody] dynamic json)
        {
            string jsonStr;
            try
            {
                jsonStr = JsonSerializer.Serialize(json);
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                string toEmail = jsonMap["Email"].ToString();
                string otp = jsonMap["Otp"].ToString();
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

        [HttpPost("signin")]
        public async Task<IActionResult> UserSignIn([FromBody] dynamic json)
        {
            try
            {
                string jsonStr = JsonSerializer.Serialize(json);
                Dictionary<string, object> jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                string toEmail = jsonMap["Email"].ToString();
                string otp = jsonMap["Otp"].ToString();
                if (!iOtpService.ValidateOtp(toEmail, otp))
                {
                    throw new Exception("Invalid otp");
                }

                string sqlScript = "spGetUserInfo @jsonStr";
                var spJsonStr = new SqlParameter("@jsonStr", SqlDbType.NVarChar, 1000) { Value = jsonStr };
                var response = await habitDBContext.Database.SqlQueryRaw<string>(sqlScript, spJsonStr).ToListAsync();

                jsonMap = JsonSerializer.Deserialize<Dictionary<string, object>>(response[0]);
                int userId = int.Parse(jsonMap["UserID"].ToString(), CultureInfo.InvariantCulture);
                var claims = new List<Claim>
                {
                    new Claim("userId", userId.ToString())
                };
                var accessToken = jwtService.GenerateJwtToken(claims);
                jsonMap.Add("Token", accessToken);
                jsonStr = JsonSerializer.Serialize(jsonMap);
                return Content(jsonStr, "application/json");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
