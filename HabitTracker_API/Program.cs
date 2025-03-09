using HabitTracker_API.Interfaces;
using HabitTracker_API.Models;
using HabitTracker_API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<HabitDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddKeyedScoped<ICreateService, AddHabitService>("habit");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
