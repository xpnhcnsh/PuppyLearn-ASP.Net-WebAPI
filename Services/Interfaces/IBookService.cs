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
        public Task<ReturnValue> AddbyFolderUrlAsync(string url);
        public Task<ReturnValue> GetWordReportsAsync(int skip, int take, CancellationToken cancellationToken);
        public Task<ReturnValue> GetBookListAsync(CancellationToken cancellationToken);
        public Task<ReturnValue> GetUserBookByIdAsync(Guid bookId, Guid userId, CancellationToken cancellationToken);
        public Task<ReturnValue> UpdateAWordReportAsync(Guid reportId, CancellationToken cancellationToken);
        public Task<ReturnValue> GetWordsByTrans(string trans, CancellationToken cancellationToken);
    }
}
