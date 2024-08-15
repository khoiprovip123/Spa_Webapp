using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IResponseCacheService
    {
        Task SetCacheResponeAsync(string cacheKey, object respone, TimeSpan timeOut);

        Task<string> GetCacheResponseAsync(string cacheKey);

        Task RemoveCacheResponseAsync(string pattern);
    }
}
