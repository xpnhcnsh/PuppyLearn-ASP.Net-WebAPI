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
        public async Task<Return2Front> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _userService.Register(registerDto, cancellationToken);
                return res;
            }
            catch(Exception ex)
            {
                return new Return2Front
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = 400
                };
            }
        }

        [HttpPost("login")]
        public async Task<Return2Front> Login([FromBody] LoginDto loginDto, CancellationToken cancellation)
        {
            try
            {
                var res = await _userService.Login(loginDto, cancellation);
                return res;
            }
            catch(Exception ex)
            {
                return new Return2Front
                {
                    Value = ex.Message,
                    Msg = "error",
                    HttpCode = 400
                };
            }
        }
    }
}
