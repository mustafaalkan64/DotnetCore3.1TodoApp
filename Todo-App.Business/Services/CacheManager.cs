using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Business.Abstract;

namespace Todo_App.Business.Services
{
    public class CacheManager : ICacheManagementService
    {
        private IMemoryCache _cache;
        private readonly ILogger<CacheManager> _logger;
        public CacheManager(IMemoryCache cache, ILogger<CacheManager> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        // I had prepared below method in order to clear all memcache with keys
        // But this method is not essential for this project.
        public async Task<bool> Clear()
        {
            try
            {
                PropertyInfo prop = _cache.GetType().GetProperty("EntriesCollection", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public);
                object innerCache = prop.GetValue(_cache);
                MethodInfo clearMethod = innerCache.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
                clearMethod.Invoke(innerCache, null);
                _logger.LogInformation("All Keys Removed From Cache");
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Error During Clear Cache");
                throw ex;
            }

        }
    }
}
