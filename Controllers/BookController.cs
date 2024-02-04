using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddbyUrl")]
        public async Task<ReturnValue> AddbyUrl([FromQuery]string url)
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

        [HttpGet("GetBookList")]
        [Authorize(Roles ="superAdmin")]
        public async Task<ReturnValue> GetBookList(CancellationToken cancellationToken)
        {
            var res = await _bookService.GetBookList(cancellationToken);
            return res;
        }

    }
}
