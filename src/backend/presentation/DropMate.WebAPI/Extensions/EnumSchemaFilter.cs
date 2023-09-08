using DropMate.Domain.Models;
using Microsoft.Extensions.DependencyModel;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using NLog.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DropMate.WebAPI.Extensions
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum = Enum.GetNames(context.Type)
                .Select(name => new OpenApiString(name))
                    .ToList<IOpenApiAny>();
                schema.Type = "string"; // Set the enum type to string
            }
        }
    }



}
