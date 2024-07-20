using Microsoft.AspNetCore.Mvc;
using PuppyLearn.Models.Dto;
using PuppyLearn.Services;
using PuppyLearn.Services.Interfaces;
using PuppyLearn.Utilities;

namespace PuppyLearn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("addbyurl")]
        [AuthorizeRoles(Roles.superAdmin)]
        public async Task<ReturnValue> AddbyUrlAsync([FromQuery]string url)
        {
            try
            {
                var res = await _bookService.AddbyFolderUrl(url);
                return res;
            }
            catch (Exception ex)
            {
                return new ReturnValue
                {
                    Value = null,
                    Msg = ex.Message,
                    HttpCode = System.Net.HttpStatusCode.BadRequest
                };

            }
        }

        [HttpGet("getbooklist")]
        [AuthorizeRoles(Roles.admin,Roles.normalUser, Roles.superAdmin, Roles.teacher, Roles.vip)]
        public async Task<ReturnValue> GetBookListAsync(CancellationToken cancellationToken)
        {
            return await _bookService.GetBookList(cancellationToken);
        }

        [HttpGet("wordreports")]
        [AuthorizeRoles(Roles.admin, Roles.superAdmin)]
        public async Task<ReturnValue> GetWordReports(CancellationToken cancellationToken)
        {
            return await _bookService.GetWordReports(cancellationToken);
        }

    }
}
