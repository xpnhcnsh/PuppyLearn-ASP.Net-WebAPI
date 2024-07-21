using PuppyLearn.Models;
using PuppyLearn.Utilities;

namespace PuppyLearn.Services.Interfaces
{
    public interface IBookService
    {
        /// <summary>
        /// Give a folder path as parameter, save all .json files inside 
        /// the folder to database
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<ReturnValue> AddbyFolderUrl(string url);
        public Task<ReturnValue> GetWordReports(CancellationToken cancellationToken);
        public Task<ReturnValue> GetBookList(CancellationToken cancellationToken);
        public Task<ReturnValue> GetUserBookById(Guid bookId, Guid userId, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateAWordReport(Guid reportId, CancellationToken cancellationToken);
    }
}
