using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PuppyLearn.Models;
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
            return await _userService.RegisterAsync(registerDto, cancellationToken);
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
				    //new Claim("SignUpTime", res.Value.SignUpTime.ToString()),
                    //new Claim("LastLoginTime", res.Value.LastLoginTime!=null ? res.Value.LastLoginTime.ToString():""),
                    new Claim(ClaimTypes.Role, Global.GetAccountTypeStr(res.Value.AccountTypeId)),
                    //new Claim("IsSuspend", res.Value.IsSuspend.ToString()),
                    //new Claim("IsValid", res.Value.IsValid.ToString()),
                    new Claim("Email", res.Value.Email),
                    //new Claim("LastLedBookId", res.Value.LastLedBookId!=null ? res.Value.LastLedBookId.ToString():"")
                };
                // 下面的securityKey存放在服务器。
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
                var jwt = new JwtSecurityToken
                    (
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                res.Msg = token;
            }
            return res;
        }

        /// <summary>
        /// 更新当前选择背诵的书List。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookDtoList"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("updatebooks/{userId}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> UpdateSelectedBooksAsync([FromRoute]Guid userId, [FromBody]List<BookDto> bookDtoList, CancellationToken cancellationToken)
        {
            return await _userService.UpdateSelectedBooksAsync(userId, bookDtoList, cancellationToken);
        }

        /// <summary>
        /// 获取用户当前选择的书List。
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("userbook/{userId}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public  async Task<ReturnValue> GetUserBookAsync([FromRoute]Guid userId, CancellationToken cancellationToken)
        {
            var res = await _userService.GetUserBooksAsync(userId, cancellationToken);
            return res;
        }

        [HttpGet("allWords/{userId}/{bookId}/{skip}/{take}/{totalRecords}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> GetAllWordsFromABookAsync([FromRoute] Guid userId, [FromRoute] Guid bookId, [FromRoute] int skip, [FromRoute] int take, [FromRoute] int totalRecords,[FromRoute] string? wordName, CancellationToken cancellationToken)
        {
            return await _userService.GetWordsFromABookAsync(userId, bookId, skip, take, totalRecords, wordName, cancellationToken);
        }

        [HttpGet("allWords/{userId}/{bookId}/{wordName}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> GetOneWordFromABookAsync([FromRoute] Guid userId, [FromRoute] Guid bookId, [FromRoute] string wordName, CancellationToken cancellationToken)
        {
            return await _userService.GetWordsFromABookAsync(userId, bookId,0,0,0, wordName, cancellationToken);
        }

        [HttpGet("oneword/{userId}/{wordName}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> GetOneWordFromAllBooksAsync([FromRoute]Guid userId, [FromRoute] string wordName, CancellationToken cancellationToken)
        {
            return await _userService.GetOneWordFromAllBookAsync(userId, wordName, cancellationToken);
        }

        [HttpPut("updateword/{userId}/{bookId}")]
        [AuthorizeRoles(Roles.admin, Roles.superAdmin)]
        public async Task<ReturnValue> UpdateWordChangesAsync([FromRoute] Guid userId, [FromRoute] Guid bookId, [FromBody] WordNFieldsDto wordNFieldsDto, CancellationToken cancellationToken)
        {
            return await _userService.UpdateAWordAsync(userId, bookId, wordNFieldsDto, cancellationToken);
        }

        [HttpDelete("delfield/{userId}/{fieldId}/{field}")]
        [AuthorizeRoles(Roles.admin, Roles.superAdmin)]
        public async Task<ReturnValue> DelFieldofAWordAsync([FromRoute] Guid userId, [FromRoute] Guid fieldId, [FromRoute] string field, CancellationToken cancellationToken)
        {
            return await _userService.DelFieldofAWordAsync(userId , fieldId, field, cancellationToken);
        }

        /// <summary>
        /// 获取List<LearnTransCardDto>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wordsCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("gettrans/{userId}/{bookId}/{wordsCount}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> LearnTransAsync([FromRoute] Guid userId, [FromRoute] Guid bookId, [FromRoute] int wordsCount, CancellationToken cancellationToken)
        {
            return await _userService.LearnTransAsync(userId, bookId, wordsCount, cancellationToken);
        }

        [HttpPost("updatesettings/{userId}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> UpdateUserSettingsAsync([FromRoute] Guid userId, [FromBody] UserSettings settings, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserSettingsAsync(userId, settings, cancellationToken);
        }

        [HttpPost("updateprogress/{userId}/{bookId}")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> UpdateProgressAsync([FromRoute] Guid userId, [FromRoute] Guid bookId, [FromBody] List<LearnTransDto> words, CancellationToken cancellationToken)
        {
            return await _userService.UpdateProgressAsync(userId, bookId, words, cancellationToken);
        }

        [HttpPost("reportword")]
        [AuthorizeRoles(Roles.admin, Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> ReportAWordAsync([FromBody] WordReportDto reportDto, CancellationToken cancellationToken)
        {
            return await _userService.ReportAWordAsync(reportDto, cancellationToken);
        }
    }
}
