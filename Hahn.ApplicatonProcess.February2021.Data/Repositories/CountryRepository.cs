using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Repositories
{
    public class CountryRepository : HttpRepositoryBase, ICountryRepository
    {
        public CountryRepository(IMapper mapper, IMemoryCache memoryCache, HttpClient client) : base(mapper, memoryCache, client)
        {}

        public async Task<IList<CountryDto>> GetAllCountriesAsync(Uri uri)
        {
            return await Get<IList<CountryDto>>(uri);
        }

        protected override async Task<T> ParseRawResponseAsync<T>(HttpContent content)
        {
            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync(), new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
