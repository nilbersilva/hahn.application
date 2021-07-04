using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Enums.Business;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Services
{
    public class SharedValuesService : ISharedValuesService
    {

        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ICountryRepository _countryRepository;
        private readonly ITopLevelDomainRepository _topLevelDomainRepository;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<SharedValuesService> _logger;

        public SharedValuesService(IMapper mapper, ICountryRepository countryRepository,
                                    ISettingsService settingsService,                                  
                                    IMemoryCache memoryCache, 
                                    ITopLevelDomainRepository topLevelDomainRepository,
                                    ILogger<SharedValuesService> logger)
        {
            _mapper = mapper;
            _countryRepository = countryRepository;
            _settingsService = settingsService;
            _memoryCache = memoryCache;
            _topLevelDomainRepository = topLevelDomainRepository;
            _logger = logger;
        }

        public async Task<IList<CountryDto>> GetAllCountriesAsync()
        {
            var excludeCountries = _settingsService.GetExcludeCountries();
            var result = await _countryRepository.GetAllCountriesAsync(_settingsService.GetRestCountriesUri());
            return result.Where(x => !excludeCountries.Contains(x.Alpha3Code)).ToList();
        }

        public async Task<IList<DepartmentDto>> GetAllDepartmentsAsync()
        {
            return await Task.FromResult(Enum.GetValues(typeof(enDepartment))
                              .Cast<enDepartment>()
                              .Select(x => _mapper.Map<DepartmentDto>(x))
                              .ToList());
        }

        public async Task<IList<string>> GetAllTopLevelDomainsAsync()
        {
            return await _topLevelDomainRepository.GetTopLevelDomainsAsync(_settingsService.GetTopLevelDomainsUri());
        }

        public void PutAllAllTopLevelDomainsInCache()
        {
            _memoryCache.Set(Constants.ALL_TOP_LEVEL_DOMAINS_CACHE_KEY, GetAllTopLevelDomainsAsync().Result);
        }

        public void PutAllCountriesInCache()
        {
            _logger.LogInformation("Putting all countries in cache.");
            _memoryCache.Set(Constants.ALL_COUNTRIES_CACHE_KEY, GetAllCountriesAsync().Result);            
        }
    }
}
