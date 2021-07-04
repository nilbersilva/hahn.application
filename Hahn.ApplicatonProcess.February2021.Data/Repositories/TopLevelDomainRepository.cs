using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Repositories
{
    public class TopLevelDomainRepository : HttpRepositoryBase, ITopLevelDomainRepository
    {
        public TopLevelDomainRepository(IMapper mapper, IMemoryCache memoryCache, HttpClient client)
                      : base(mapper, memoryCache, client)
        { }

        public async Task<IList<string>> GetTopLevelDomainsAsync(Uri uri)
        {
            return await Get<IList<string>>(uri);
        }

        protected override async Task<T> ParseRawResponseAsync<T>(HttpContent content)
        {
            string contentText = await content.ReadAsStringAsync();
            var resultList = contentText.Split("\n")
                                                .Skip(1)    //First Line is just a Header for the file, example: # Version 2021063000, Last Updated Wed Jun 30 07:07:01 2021 UTC
                                                .Select(x => (x != null ? $".{x.ToLower()}" : string.Empty))
                                                .ToList() as T;
            return resultList;
        }
    }
}
