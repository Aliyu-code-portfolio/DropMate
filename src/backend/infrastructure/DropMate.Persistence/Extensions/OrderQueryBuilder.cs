using DropMate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DropMate.Persistence.Extensions
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Split(',');
            var properties = typeof(User).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var queryProperty = param.Split(' ')[0];
                var property = properties.FirstOrDefault(p => p.Name.Equals(queryProperty, StringComparison.InvariantCultureIgnoreCase));
                if (property is null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{property.Name.ToString()} {direction}");
            }
            return orderQueryBuilder.ToString().TrimEnd(' ');
        }
    }
}
