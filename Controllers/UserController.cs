using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;

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
        public async Task<ReturnValue> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _userService.Register(registerDto, cancellationToken);
                return res;
            }
            catch(Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = 400
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ReturnValue> Login([FromBody] LoginDto loginDto, CancellationToken cancellation)
        {
            try
            {
                var res = await _userService.Login(loginDto, cancellation);
                return res;
            }
            catch(Exception ex)
            {
                return new ReturnValue
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = 400
                };
            }
        }
    }
}
