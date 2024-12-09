using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next,ILogger<GlobalExceptionHandler> logger){
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context){
            try
            {
                await _next(context);
                
            }
            catch(CustomExceptionClass ex){
                _logger.LogError($"{ex.Message}");
                HandleExceptionAsync(context,ex.Message.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                HandleNormalExceptionAsync(context,"Unexpected Error Occurs");
            }
        }

        private Task HandleExceptionAsync(HttpContext context,string message){
            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
            var errorMessage = Uri.EscapeDataString(message);
            var redirectUrl = $"{context.Request.Path}{queryString}&error={errorMessage}";
            context.Response.Redirect(redirectUrl);
            return Task.CompletedTask;

        }
        private Task HandleNormalExceptionAsync(HttpContext context,string message){
             _logger.LogError(message);

            var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
            var errorMessage = Uri.EscapeDataString(message);
            var redirectUrl = $"/NormalExceptionPage/?error={errorMessage}{queryString}";

            context.Response.Redirect(redirectUrl);
            return Task.CompletedTask;

        }
    }
}