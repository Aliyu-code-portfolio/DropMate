using DropMate.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Extensions
{
    public static class ReviewPlanRepositoryExtension
    {
        public static IQueryable<Review> Sort(this IQueryable<Review> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query;
            var orderQueryBuilder = OrderQueryBuilder.CreateOrderQuery<Review>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query;
            return query.OrderBy(orderQueryBuilder);
        }
    }
}
