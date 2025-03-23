using System.Security.Claims;

namespace HabitTracker_API.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(List<Claim> claims);
    }
}
