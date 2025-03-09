using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HabitTracker_API.Models
{
    public class HabitDBContext : DbContext
    {
        public HabitDBContext(DbContextOptions options) : base(options) { }
    }
}
