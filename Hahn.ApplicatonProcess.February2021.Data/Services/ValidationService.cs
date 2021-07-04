using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ValidationService(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public ValidationResult Validate<U, V>(U item) where V : AbstractValidator<WithMemoryCache<AssetDto>>, new()
        {
            return CommonValidate<AssetDto, U, V>(item);
        }

        private ValidationResult CommonValidate<T, U, V>(U item) where V : AbstractValidator<WithMemoryCache<T>>, new() where T : new()
        {
            var mapped = _mapper.Map<T>(item);
            var validator = new V();
            var model = new WithMemoryCache<T>
            {
                Instance = mapped,
                MemoryCache = _memoryCache
            };
            return validator.Validate(model);
        }
    }
}
