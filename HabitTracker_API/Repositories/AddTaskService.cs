using HabitTracker_API.Interfaces;
using HabitTracker_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace HabitTracker_API.Repositories
{
    public class AddTaskService : ControllerBase, IAddTaskService
    {
        private readonly HabitDBContext habitDBContext;
        public AddTaskService(HabitDBContext habitDBContext)
        {
            this.habitDBContext = habitDBContext;
        }
        public async Task<IActionResult> AddTask(dynamic json)
        {
            try
            {
                string jsonStr = JsonSerializer.Serialize(json);
                string sqlScript = "exec spAddHabit @jsonStr, @status out, @statusMsg out";
                var spJsonStr = new SqlParameter("@jsonStr", SqlDbType.NVarChar, 500) { Value = jsonStr };
                var spStatus = new SqlParameter("@status", SqlDbType.TinyInt) { Direction = ParameterDirection.Output };
                var spStatusMsg = new SqlParameter("@statusMsg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output };
                await habitDBContext.Database.ExecuteSqlRawAsync(sqlScript, spJsonStr, spStatus, spStatusMsg);
                
                int status = (int)spStatus.Value;
                string statusMsg = spStatusMsg.Value.ToString();
                if(status == 0)
                    throw new Exception(statusMsg);
                return Content("Success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
