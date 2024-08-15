using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.MIiddleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            context.Response.OnStarting(() =>
            {
                watch.Stop();
                var responseTime = watch.ElapsedMilliseconds;
                context.Response.Headers.Add("X-Response-Time-ms", responseTime.ToString());
                _logger.LogInformation($"Request [{context.Request.Method}] at {context.Request.Path} took {responseTime} ms");
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
