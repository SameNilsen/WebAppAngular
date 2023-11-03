using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAll();
        IEnumerable<Post>? GetBySubForum(string forum);
        Task<Post?> GetItemById(int id);
        Task<bool> Create(Post item);
        Task<bool> Update(Post item);
        Task<bool> Delete(int id);
    }
}
