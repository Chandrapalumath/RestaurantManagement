using RestaurantManagement.Backend.Exceptions;
using System.Net;
using System.Text.Json;

namespace RestaurantManagement.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Success = false,
                    Message = ex.Message
                };

                context.Response.StatusCode = ex switch
                {
                    BadRequestException => (int)HttpStatusCode.BadRequest,       // 400
                    NotFoundException => (int)HttpStatusCode.NotFound,           // 404
                    UnauthorizedException => (int)HttpStatusCode.Unauthorized,   // 401
                    _ => (int)HttpStatusCode.InternalServerError                 // 500
                };

                response.StatusCode = context.Response.StatusCode;

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
