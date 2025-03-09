using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker_API.Interfaces
{
    public interface ICreateService
    {
        Task<IActionResult> Create(dynamic json);
    }
}
