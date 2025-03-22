using Microsoft.AspNetCore.Mvc;

namespace HabitTracker_API.Interfaces
{
    public interface ISPService
    {
        Task<IActionResult> ExecuteSP(string sqlScript, string jsonStr);
    }
}
