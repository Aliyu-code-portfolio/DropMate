using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Extensions
{
    public static class UserRepositoryExtension
    {
        public static IQueryable<User> Sort(this IQueryable<User> query, string orderByQueryString) 
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderBy(u => u.FirstName);
            var orderQueryBuilder = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);    
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderBy(u => u.FirstName);
            return query.OrderBy(orderQueryBuilder);
        }
    }
}
