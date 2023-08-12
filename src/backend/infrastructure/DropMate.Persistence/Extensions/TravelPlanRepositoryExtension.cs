using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Extensions
{
    public static class TravelPlanRepositoryExtension
    {
        public static IQueryable<TravelPlan> Sort(this IQueryable<TravelPlan> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderByDescending(u => u.MaximumPackageWeight);
            var orderQueryBuilder = OrderQueryBuilder.CreateOrderQuery<TravelPlan>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query.OrderByDescending(u => u.CreatedDate);
            return query.OrderBy(orderQueryBuilder);
        }
    }
}
