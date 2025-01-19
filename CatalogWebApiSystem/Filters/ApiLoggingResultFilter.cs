using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CatalogWebApiSystem.Filters
{
    public class ApiLoggingResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<ApiLoggingResultFilter> _logger;

        public ApiLoggingResultFilter(ILogger<ApiLoggingResultFilter> logger)
        {
            _logger = logger;
        }



        public void OnResultExecuting(ResultExecutingContext context)
        {
            var now = GetNowLongTimeString();
            _logger.LogInformation($"{now} - action executing {context.HttpContext.Request.Method} - {context.HttpContext.Request.Path}");
            _logger.LogInformation($"ModelState {context.ModelState.IsValid}");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var now = GetNowLongTimeString();
            var statusCode = context.HttpContext.Response.StatusCode;

            _logger.LogInformation($"{now} - action executed {context.HttpContext.Request.Method} - {context.HttpContext.Request.Path}");

            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
                return;

            // Converte o inteiro para HttpStatusCode e exibe o nome
            HttpStatusCode httpStatus = (HttpStatusCode)statusCode;

            var logLevel = LogLevel.Information;

            if (IsStatusCodeClientSideError(statusCode))
                logLevel = LogLevel.Warning;
            else if (IsStatusCodeServerSideError(statusCode))
                logLevel = LogLevel.Error;


            _logger.Log(logLevel, "StatusCode: {statusCode} {httpStatus}", statusCode, httpStatus);
        }

        private static string GetNowLongTimeString() => DateTime.Now.ToLongTimeString();

        private static bool IsStatusCodeClientSideError(int statusCode) =>
            statusCode >= 400 && statusCode < 500;

        private static bool IsStatusCodeServerSideError(int statusCode) =>
            statusCode >= 500 && statusCode < 600;
    }
}
