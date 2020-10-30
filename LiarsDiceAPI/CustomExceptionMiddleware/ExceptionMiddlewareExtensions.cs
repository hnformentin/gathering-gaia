﻿using Microsoft.AspNetCore.Builder;

namespace LiarsDiceAPI.CustomExceptionMiddleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
        }
    }
}
