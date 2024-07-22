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
            CreateMap<Cognate, CognatesDto>().ForMember(
                dest => dest.Pos,
                opt => opt.MapFrom(src => src.Pos.Trim())); ;
            CreateMap<Phrase, PhraseDto>();
            CreateMap<RemMethod, RemMethodDto>();
            CreateMap<Sentence, SentenceDto>();
            CreateMap<SingleChoiceQuestion, SingleChoiceQuestionDto>();
            CreateMap<Synonymou, SynonymousDto>().ForMember(
                dest => dest.Pos,
                opt => opt.MapFrom(src => src.Pos.Trim()));
            CreateMap<Tran, TranDto>().ForMember(
                dest => dest.Pos,
                opt => opt.MapFrom(src => src.Pos.Trim()));

            CreateMap<Word, WordDto>()
                .ForMember(
                dest => dest.BookNameCh,
                opt => opt.MapFrom(src => src.Book.BookNameCh))
                .ForMember(
                dest => dest.Cognates,
                opt => opt.MapFrom(src => src.Cognates))
                .ForMember(
            dest => dest.Phrases,
                opt => opt.MapFrom(src => src.Phrases))
                .ForMember(
            dest => dest.RemMethods,
                opt => opt.MapFrom(src => src.RemMethods))
                .ForMember(
            dest => dest.Sentences,
                opt => opt.MapFrom(src => src.Sentences))
                .ForMember(
            dest => dest.SingleChoiceQuestions,
                opt => opt.MapFrom(src => src.SingleChoiceQuestions))
                .ForMember(
            dest => dest.Trans,
                opt => opt.MapFrom(src => src.Trans));

            CreateMap<WordReportPostDto, WordReport>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(
                dest => dest.SubmitTime,
                opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
