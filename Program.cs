using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PuppyLearn.Models;
using PuppyLearn.Profiles;
using PuppyLearn.Services;
using PuppyLearn.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<PuppylearnContext>(
    options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PuppyLearn")));

// AutoMapper register
builder.Services.AddAutoMapper(typeof(UserProfiles));

// Service register
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();

// 使service层可以获取HttpContext，从而获取用户信息
builder.Services.AddHttpContextAccessor();
AppServiceHelper.Initialize(builder.Configuration);

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer
        (
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateLifetime = true,
                // jwt默认对设置的expiretime会给一个offside，这里将offside置零，否则设置的expiretime会不按照期望生效。
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
            };
        });


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
