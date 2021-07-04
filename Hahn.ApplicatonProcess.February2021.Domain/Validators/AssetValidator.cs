using FluentValidation;
using Hahn.ApplicatonProcess.February2021.Domain.Enums.Business;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Validators
{
    public class AssetValidator : AbstractValidator<WithMemoryCache<AssetDto>>
    {
        public AssetValidator()
        {
            RuleFor(x => x.Instance.AssetName).NotEmpty();

            RuleFor(x => x.Instance.AssetName).MinimumLength(5);

            RuleFor(x => x.Instance.Department).NotEmpty();

            Transform(x => x.Instance.Department, x => (enDepartment)int.Parse(x)).IsInEnum();

            RuleFor(x => x.Instance.Country).NotEmpty();

            RuleFor(x => x).Must(x =>
            {
                if (x.MemoryCache.TryGetValue(Constants.ALL_COUNTRIES_CACHE_KEY, out List<CountryDto> countries))
                {
                    return countries.Exists(y => y.Alpha3Code == x.Instance.Country);
                }
                return false;
            }).WithMessage("Wrong country code.");

            RuleFor(x => x.Instance.PurchaseDate).NotEmpty();

            RuleFor(x => x.Instance.PurchaseDate)
                .InclusiveBetween(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow);

            RuleFor(x => x.Instance.Email).NotEmpty();

            RuleFor(x => x).Must(x =>
            {
                if (x.MemoryCache.TryGetValue(Constants.ALL_TOP_LEVEL_DOMAINS_CACHE_KEY, out List<string> topLevelDomains))
                {
                    return topLevelDomains.Exists(y => !string.IsNullOrEmpty(x.Instance.Email) && x.Instance.Email.EndsWith(y));
                }
                return false;
            }).WithMessage("Wrong top level domain in email address.");
        }
    }
}
