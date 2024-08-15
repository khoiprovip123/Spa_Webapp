using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spa.Application.Configuration;
using Spa.Application.Configuration;
using Spa.Domain.IService;
using System.Text;

namespace Spa.Api.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToliveSeconds;
        public CacheAttribute(int timeToLiveSeconds = 1000)
        {
            _timeToliveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            if (!cacheConfiguration.Enabled)
            {
                await next();
                return;
            }

            IResponseCacheService cacheService = null;
            try
            {
                cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            }
            catch (StackExchange.Redis.RedisConnectionException ex)
            {
                Console.WriteLine("Redis connection failed: " + ex.Message);
                await next();
                return;
            }// sử dụng service đã DI
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            if (cacheResponse != null)
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult objectResult)
            {
                await cacheService.SetCacheResponeAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToliveSeconds));
            }
        }
        
        public static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"{key}--{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
