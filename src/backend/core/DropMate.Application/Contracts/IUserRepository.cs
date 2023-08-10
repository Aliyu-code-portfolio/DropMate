using DropMate.Domain.Models;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;

namespace DropMate.Application.Contracts
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetAllUsersAsync(UserRequestParameters requestParameter, bool trackChanges);
        Task<User> GetByIdAsync(string id, bool trackChanges);
        Task<User> GetByEmailAsync(string email, bool trackChanges);
        void UpdateUser(User user);
        void DeleteUser(User user);
        void PermanentDeleteUser(User user);
        void CreateUser(User user);
    }
}
