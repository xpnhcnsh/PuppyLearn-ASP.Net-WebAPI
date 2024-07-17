using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PuppyLearn.Models;
using PuppyLearn.Profiles;
using PuppyLearn.Services;
using PuppyLearn.Services.Interfaces;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<PuppyLearnContext>(
    options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PuppyLearn"),
    sqlServerOptions =>
    {
        sqlServerOptions.CommandTimeout(16);
        sqlServerOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null);
    }));

// AutoMapper register
builder.Services.AddAutoMapper(typeof(UserProfiles));
builder.Services.AddAutoMapper(typeof(BookProfiles));

// Service register
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();

// ʹservice����Ի�ȡHttpContext���Ӷ���ȡ�û���Ϣ
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
                // jwtĬ�϶����õ�expiretime���һ��offside�����ｫoffside���㣬�������õ�expiretime�᲻����������Ч��
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        };
        });


var app = builder.Build();

app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
