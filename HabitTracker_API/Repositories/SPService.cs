using HabitTracker_API.Interfaces;
using HabitTracker_API.EFContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace HabitTracker_API.Repositories
{
    public class SPService : ControllerBase, ISPService
    {
        private readonly HabitDBContext habitDBContext;
        public SPService(HabitDBContext habitDBContext)
        {
            this.habitDBContext = habitDBContext;
        }
        public async Task<IActionResult> ExecuteSP(string sqlScript, string jsonStr)
        {
            try
            {
                sqlScript += " @jsonStr";
                var spJsonStr = new SqlParameter("@jsonStr", SqlDbType.NVarChar, 1000) { Value = jsonStr };
                var response = await habitDBContext.Database.SqlQueryRaw<string>(sqlScript, spJsonStr).ToListAsync();
                if(response.Count == 0)
                {
                    return Content("{}", "application/json");
                }

                var jsonMap = JsonSerializer.Deserialize<Dictionary<string,object>>(response[0]);
                if(jsonMap.ContainsKey("error"))
                {
                    throw new Exception(response[0]);
                }

                return Content(response[0], "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
