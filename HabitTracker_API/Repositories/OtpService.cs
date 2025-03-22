using HabitTracker_API.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace HabitTracker_API.Repositories
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;
        public OtpService(IConfiguration configuration, IMemoryCache cache)
        {
            this.configuration = configuration;
            this.cache = cache;
        }
        public string GenerateOtp()
        {
            int n = configuration.GetValue<int>("OTP:Length");
            string key = configuration.GetValue<string>("OTP:Key");
            string otp = "";
            for(int i = 1; i <= n; i++)
            {
                Random random = new Random();
                otp += key[random.Next() % key.Length];
            }
            return otp;
        }
        public async Task SendOtpViaEmail(string toEmail, string otp)
        {
            string smtpServer = configuration.GetValue<string>("OTP:SMTP");
            int port = configuration.GetValue<int>("OTP:Port");
            string serviceEmail = configuration.GetValue<string>("OTP:Email");
            string password = configuration.GetValue<string>("OTP:Password");
            int expiry = configuration.GetValue<int>("OTP:Expiry");
            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(serviceEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(serviceEmail),
                Subject = "HabitTracker Authentication",
                Body = $"Your OTP(one time password) for authentication is <b>{otp}</b>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(toEmail));
            cache.Set(toEmail, otp, TimeSpan.FromMinutes(expiry));
            await smtpClient.SendMailAsync(mailMessage);
        }
        public bool ValidateOtp(string email, string otp)
        {
            if(cache.TryGetValue(email, out string otpCache) && otpCache == otp)
            {
                cache.Remove(email);
                return true;
            }
            return false;
        }
    }
}
