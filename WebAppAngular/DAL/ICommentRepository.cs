using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>?> GetAll();
        Task<Comment?> GetCommentById(int id); 
        IEnumerable<Comment>? GetCommentsByPostId(int id);

        Task<bool> Create(Comment comment);
        Task<bool> Update(Comment comment);
        Task<bool> Delete(int id);
    }
}
