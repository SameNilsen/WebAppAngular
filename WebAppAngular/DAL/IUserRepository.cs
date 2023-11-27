using OsloMetAngular.Models;

namespace OsloMetAngular.DAL
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>?> GetAll();
        Task<User?> GetItemById(int id);
        Task<User?> GetUserByIdentity(string id);
        Task<bool> Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);
    }
}
