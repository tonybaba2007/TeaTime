﻿namespace TeaTime.Common.Cache
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using MediatR;

    public class CacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICache _cache;

        public CacheBehaviour(ICache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!(request is ICacheableQuery cacheQuery))
                return await next();

            var cacheKey = cacheQuery.CacheKey;

            // try and get from the cache first
            var cacheItem = await _cache.GetAsync<TResponse>(cacheKey, cancellationToken);
            if (cacheItem != null)
                return cacheItem.Value;

            // no result found in cache, so run the rest of the pipeline and cache the result
            var response = await next();

            // dont cache null values
            if (response == null)
                return default;

            // serialize and store in the cache
            await _cache.SetAsync(cacheKey, response, new CacheEntryOptions
            {
                Sliding = cacheQuery.SlidingCache,
                Expiration = cacheQuery.CacheExpiry
            }, cancellationToken);

            return response;
        }
    }
}