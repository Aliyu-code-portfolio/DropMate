using DropMate.Domain.Models;

namespace DropMate.Application.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
        Task<User> GetByIdAsync(string id, bool trackChanges);
        Task<User> GetByEmailAsync(string email, bool trackChanges);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void CreateUser(User user);
    }
}
