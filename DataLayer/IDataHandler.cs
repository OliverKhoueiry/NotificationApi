using CommonLayer.Models;

namespace DataLayer
{
    public interface IDataHandler
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> AddUserAsync(User user);
    }
}
