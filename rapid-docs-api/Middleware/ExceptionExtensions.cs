using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using rapid_docs_api.Utilities;
using System.Net;


namespace rapid_docs_api.Middleware
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Global Error handling, intercept and return error
        /// From https://code-maze.com/global-error-handling-aspnetcore/
        /// </summary>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // Add Logging Later
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        var error = contextFeature.Error;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = error?.Message ?? "" + " - " + error?.StackTrace + " - " + error?.InnerException  ?? ""
                        }.ToString());
                    }
                });
            });
        }
    }
}