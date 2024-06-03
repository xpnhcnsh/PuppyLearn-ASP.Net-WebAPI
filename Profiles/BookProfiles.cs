using AutoMapper;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;

namespace PuppyLearn.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<BooksEn, BookDto>();
        }
    }
}
