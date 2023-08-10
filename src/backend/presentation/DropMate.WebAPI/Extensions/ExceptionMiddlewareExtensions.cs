using DropMate.Application.Common;
using DropMate.Shared.ErrorModels;
using DropMate.Shared.Exceptions.Base;
using Microsoft.AspNetCore.Diagnostics;

namespace DropMate.WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            BadRequestException =>StatusCodes.Status400BadRequest,
                            NotFoundException => StatusCodes.Status404NotFound,
                            NotAlterableException => StatusCodes.Status406NotAcceptable,
                            InvalidCodeException => StatusCodes.Status403Forbidden,
                            _ => StatusCodes.Status500InternalServerError
                        }; ;
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                });
            });
        }
    }
}
