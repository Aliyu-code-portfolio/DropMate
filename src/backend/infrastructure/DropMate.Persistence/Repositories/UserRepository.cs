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
        //check for deleted before pulling user
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

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).Where(u=> !u.IsDeleted).ToListAsync();
        }

        public Task<User> GetByEmailAsync(string email, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(string id, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
