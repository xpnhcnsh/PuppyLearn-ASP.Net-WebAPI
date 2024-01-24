using AutoMapper;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Utilities;

namespace PuppyLearn.Services.Interfaces
{
    public interface IUserService
    {
        public  Task<ReturnValue> Register(RegisterDto registerDto, CancellationToken cancellationToken);
        public  Task<ReturnValue> Login(LoginDto loginDto, CancellationToken cancellationToken);
    }

}

