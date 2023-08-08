using DropMate.Application.Contracts;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Repositories
{
    internal sealed class UserRepository :RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repository):base(repository)
        {
            
        }
        public void CreateUser(User user)
        {
            Add(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public void PermanentDeleteUser(User user)
        {
            PermanentDelete(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Where(u=> !u.IsDeleted).Include(u=>u.TravelPlans).Include(u=>u.Packages).ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email, bool trackChanges)
        {
            return await FindByCondition(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)
            && !u.IsDeleted, trackChanges).Where(u=>!u.IsDeleted).Include(u => u.TravelPlans).Include(u => u.Packages)
            .FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(string id, bool trackChanges)
        {
            return await FindByCondition(u => u.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)
            && !u.IsDeleted, trackChanges).Where(u => !u.IsDeleted).Include(u => u.TravelPlans).Include(u => u.Packages)
            .FirstOrDefaultAsync();
        }


        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
