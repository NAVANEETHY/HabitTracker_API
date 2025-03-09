using HabitTracker_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly IAddTaskService iAddTaskService;
        public HabitsController(IAddTaskService iAddTaskService)
        {
            this.iAddTaskService = iAddTaskService;
        }

        [HttpPost("add")]
        public Task<IActionResult> AddHabit(dynamic json)
        {
            return iAddTaskService.AddTask(json);
        }
    }
}
