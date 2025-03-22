using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HabitTracker_API.Interfaces
{
    public interface IOtpService
    {
        string GenerateOtp();
        Task SendOtpViaEmail(string toEmail, string otp);

        bool ValidateOtp(string email, string otp);
    }
}
