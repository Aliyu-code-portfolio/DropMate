using DropMate.Application.Contracts;
using DropMate.Domain.Models;
using DropMate.Persistence.Common;
using DropMate.Persistence.Extensions;
using DropMate.Shared.RequestFeature;
using DropMate.Shared.RequestFeature.Common;
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

        public async Task<PagedList<User>> GetAllUsersAsync(UserRequestParameters requestParameters, bool trackChanges)
        {
            List<User> users = await FindAll(trackChanges).Where(u=> !u.IsDeleted 
            && u.DateJoined>=requestParameters.MinJoinDate && u.DateJoined<=requestParameters.MaxJoinDate)
                .Skip((requestParameters.PageNumber-1)*requestParameters.PageSize).Take(requestParameters.PageSize)
                .Sort(requestParameters.OrderBy)
                .Include(u=>u.TravelPlans).Include(u=>u.Packages).ToListAsync();
            int count = await FindAll(trackChanges).Where(u => !u.IsDeleted && u.DateJoined >= requestParameters.MinJoinDate 
            && u.DateJoined <= requestParameters.MaxJoinDate).CountAsync();
            return new PagedList<User>(users, count,requestParameters.PageNumber,requestParameters.PageSize);
        }

        public async Task<User> GetByEmailAsync(string email, bool trackChanges)
        {
            return await FindByCondition(u => u.Email.ToLower().Contains(email.ToLower())
            && !u.IsDeleted, trackChanges).Where(u=>!u.IsDeleted).Include(u => u.TravelPlans).Include(u => u.Packages)
            .FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(string id, bool trackChanges)
        {
            return await FindByCondition(u => u.Id.ToLower().Contains(id.ToLower())
            && !u.IsDeleted, trackChanges).Where(u => !u.IsDeleted).Include(u => u.TravelPlans).Include(u => u.Packages)
            .FirstOrDefaultAsync();
        }


        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
