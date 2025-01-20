using CatalogWebApiSystem.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CatalogWebApiSystem.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {

        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            context.Result = new ObjectResult(ApiErrorMessages.UnableToRecognizeRequest)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
