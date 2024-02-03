using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;

namespace PuppyLearn.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<RegisterDto, User>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));

            CreateMap<User, UserDto>();

            CreateMap<LoginDto, UserDto>();
        }
    }
}
