using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Repositories
{
    public abstract class HttpRepositoryBase
    {
        protected IMapper Mapper { get; private set; }
        protected IMemoryCache MemoryCache { get; private set; }
        protected HttpClient HttpClient { get; private set; }

        public HttpRepositoryBase(IMapper mapper, IMemoryCache memoryCache, HttpClient client)
        {
            MemoryCache = memoryCache;
            HttpClient = client;
            Mapper = mapper;
        }

        protected async Task<T> Get<T>(Uri uri) where T : class
        {
            T result;

            if (MemoryCache.TryGetValue(uri, out string cachedText))
            {
                result = System.Text.Json.JsonSerializer.Deserialize<T>(cachedText);
            }
            else
            {
                var response = await HttpClient.GetAsync(uri);

                result = await ParseRawResponseAsync<T>(response.Content);

                //Save only necessary info
                MemoryCache.Set(uri, System.Text.Json.JsonSerializer.Serialize(result));
            }

            return result;
        }

        protected abstract Task<T> ParseRawResponseAsync<T>(HttpContent content) where T : class;
    }
}
