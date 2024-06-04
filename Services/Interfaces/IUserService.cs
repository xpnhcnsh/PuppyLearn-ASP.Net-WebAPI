using AutoMapper;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Utilities;

namespace PuppyLearn.Services.Interfaces
{
    public interface IUserService
    {
        public  Task<ReturnValue> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        public  Task<ReturnValue> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        public Task<ReturnValue> AddNewBooks(UserDto userDto, List<BookDto> bookDtoList, CancellationToken cancellationToken);
    }

}

