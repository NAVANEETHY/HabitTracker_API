using HabitTracker_API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : ControllerBase
    {
        private readonly ICreateService iCreateService;
        public HabitsController([FromKeyedServices("habit")] ICreateService iCreateService)
        {
            this.iCreateService = iCreateService;
        }

        [HttpPost("add")]
        public Task<IActionResult> AddHabit(dynamic json)
        {
            return iCreateService.Create(json);
        }
    }
}
