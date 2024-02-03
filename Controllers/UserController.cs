using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.Net;

namespace PuppyLearn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ReturnValue> RegisterAsync([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var res = await _userService.RegisterAsync(registerDto, cancellationToken);
            return res;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ReturnValue> LoginAsync([FromBody] LoginDto loginDto, CancellationToken cancellation)
        {
            var res = await _userService.LoginAsync(loginDto, cancellation);
            return res;
            
        }
    }
}
