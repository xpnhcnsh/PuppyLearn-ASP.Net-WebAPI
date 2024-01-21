using AutoMapper;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Utilities;

namespace PuppyLearn.Services.Interfaces
{
    public interface IUserService
    {
        public  Task<Return2Front> Register(RegisterDto registerDto, CancellationToken cancellationToken);
        public  Task<Return2Front> Login(LoginDto loginDto, CancellationToken cancellationToken);
    }

}

