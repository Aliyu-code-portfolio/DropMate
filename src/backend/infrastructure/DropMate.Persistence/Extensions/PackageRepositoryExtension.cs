using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Extensions
{
    public static class PackageRepositoryExtension
    {
        public static IQueryable<Package> Sort(this IQueryable<Package> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderByDescending(u => u.Price);
            var orderQueryBuilder = OrderQueryBuilder.CreateOrderQuery<Package>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderByDescending(u => u.Price);
            return query.OrderBy(orderQueryBuilder);
        }
    }
}
