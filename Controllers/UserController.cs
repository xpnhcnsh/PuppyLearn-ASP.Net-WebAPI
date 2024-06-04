using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PuppyLearn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        public UserController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [AllowAnonymous]
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
            if (res.HttpCode==HttpStatusCode.OK)
            {
                var claims = new List<Claim>
                 {
                    new Claim("UserId", res.Value.Id.ToString()),
                    new Claim("UserName", res.Value.UserName),
				    new Claim("SignUpTime", res.Value.SignUpTime.ToString()),
                    new Claim("LastLoginTime", res.Value.LastLoginTime!=null ? res.Value.LastLoginTime.ToString():""),
                    new Claim(ClaimTypes.Role, Global.GetAccountTypeStr(res.Value.AccountTypeId)),
                    new Claim("IsSuspend", res.Value.IsSuspend.ToString()),
                    new Claim("IsValid", res.Value.IsValid.ToString()),
                    new Claim("Email", res.Value.Email),
                    new Claim("LastLedBookId", res.Value.LastLedBookId!=null ? res.Value.LastLedBookId.ToString():"")
                };
                // 下面的securityKey存放在服务器。
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
                var jwt = new JwtSecurityToken
                    (
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(7),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                res.Msg = token;
            }
            return res;
        }

        [HttpPost("addBooks/{userId}")]
        public async Task<ReturnValue> AddNewBooksAsync([FromRoute]Guid userId, [FromBody]List<BookDto> bookDtoList, CancellationToken cancellationToken)
        {
            var res = await _userService.AddNewBooksAsync(userId, bookDtoList, cancellationToken);
            return res;
        }

        [HttpPost("userBook/{userId}")]
        public  async Task<ReturnValue> GetUserBook([FromRoute]Guid userId, CancellationToken cancellationToken)
        {
            var res = await _userService.GetUserBooksAsync(userId, cancellationToken);
            return res;
        }
    }
}
