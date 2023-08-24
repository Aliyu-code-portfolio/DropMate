using DropMate2.Application.Common;
using DropMate2.Shared.HelperModels;
using DropMate2.Shared.Exceptions.Base;
using Microsoft.AspNetCore.Diagnostics;

namespace DropMate2.WebAPI.Extensions
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
                            FailedException => StatusCodes.Status406NotAcceptable,
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
