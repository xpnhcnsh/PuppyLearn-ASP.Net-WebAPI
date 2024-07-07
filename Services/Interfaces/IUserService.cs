using Microsoft.AspNetCore.Mvc;
using PuppyLearn.Models;
using PuppyLearn.Models.Dto;
using PuppyLearn.Utilities;


namespace PuppyLearn.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ReturnValue> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        public Task<ReturnValue> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateSelectedBooksAsync(Guid userId, List<BookDto> bookDtoList, CancellationToken cancellationToken);
        public Task<ReturnValue> GetUserBooksAsync(Guid userId, CancellationToken cancellationToken);
        public Task<ReturnValue> GetWordsFromABookAsync(Guid userId, Guid bookId, int skip, int take, int totalRecords, string? wordName, CancellationToken cancellationToken);
        public Task<ReturnValue> GetOneWordFromAllBookAsync(Guid userId, string wordName, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateAWordAsync(Guid userId, Guid bookId, WordNFieldsDto wordNFieldsDto, CancellationToken cancellationToken);
        public Task<ReturnValue> DelFieldofAWordAsync(Guid userId, Guid fieldId, string field, CancellationToken cancellationToken);
        public Task<ReturnValue> LearnTransAsync(Guid userId, Guid bookId, int wordsCount, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateUserSettings(Guid userId,  UserSettings settings, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateProgress(Guid userId, Guid bookId, List<LearnTransDto> words, CancellationToken cancellationToken);
    }
}

