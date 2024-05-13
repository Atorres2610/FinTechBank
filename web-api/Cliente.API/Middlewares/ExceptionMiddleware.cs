using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Cliente.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation("{EnvironmentName}", environment.EnvironmentName);

            try
            {
                await next(context);
            }
            catch (TaskCanceledException ex)
            {
                logger.LogInformation(ex, "Solicitud cancelada.");

                ProblemDetails problemDetails = new()
                {
                    Title = "Solicitud cancelada",
                    Detail = "El usuario realizó una solicitud a la página, pero luego la canceló.",
                    Status = StatusCodes.Status202Accepted,
                };

                await HandleExceptionAsync(context, StatusCodes.Status202Accepted, JsonSerializer.Serialize(problemDetails));
            }
            catch (DbUpdateException ex)
            {
                string mensajeError = ex.InnerException is not null ? ex.InnerException.Message : ex.Message;
                logger.LogError("Error: {mensajeError}", mensajeError);
                ProblemDetails problemDetails = new()
                {
                    Title = "No se puede procesar la entidad",
                    Detail = mensajeError,
                    Status = StatusCodes.Status422UnprocessableEntity,
                };

                await HandleExceptionAsync(context, StatusCodes.Status422UnprocessableEntity, JsonSerializer.Serialize(problemDetails));
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {Message}", ex.Message);
                ProblemDetails problemDetails = new()
                {
                    Title = "Ocurrió un error inesperado",
                    Detail = $"Error: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError,
                };

                await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, JsonSerializer.Serialize(problemDetails));
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(response, Encoding.UTF8);
        }
    }
}