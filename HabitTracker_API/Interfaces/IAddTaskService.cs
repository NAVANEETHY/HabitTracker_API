using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker_API.Interfaces
{
    public interface IAddTaskService
    {
        Task<IActionResult> AddTask(dynamic json);
    }
}
