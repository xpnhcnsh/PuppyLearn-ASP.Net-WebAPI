using Microsoft.EntityFrameworkCore;
using PuppyLearn.Models;
using PuppyLearn.Profiles;
using PuppyLearn.Services;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<PuppyLearnContext>(
    options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PuppyLearn")));

// AutoMapper register
builder.Services.AddAutoMapper(typeof(UserProfiles));

// Service register
builder.Services.AddScoped<IUserService, UserService>();

// 使service层可以获取HttpContext，从而获取用户信息
builder.Services.AddHttpContextAccessor();
AppServiceHelper.Initialize(builder.Configuration);


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
