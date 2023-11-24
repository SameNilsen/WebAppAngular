using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public interface IUpVoteRepository
    {
        Task<IEnumerable<Upvote>?> GetAll();
        Task<Upvote?> GetVoteById(int id); 
        IEnumerable<Upvote>? GetVotesByPostId(int id);
        Task<bool> Create(Upvote vote);
        Task<bool> Update(Upvote vote);
        Task<bool> Delete(int id);
    }
}
