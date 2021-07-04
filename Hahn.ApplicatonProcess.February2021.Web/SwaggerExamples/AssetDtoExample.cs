using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Web.SwaggerExamples
{
    public class AssetDtoExampleNew : IExamplesProvider<AssetDto>
    {
        public AssetDto GetExamples()
        {
            return new AssetDto()
            {
                AssetName = "Super computer",
                Department = "1",
                Country = "BRA",
                Email = "email@email.com",
                PurchaseDate = DateTimeOffset.UtcNow.Date,
                Broken = false
            };
        }
    }

    public class AssetDtoExampleEdit : IExamplesProvider<AssetDto>
    {
        public AssetDto GetExamples()
        {
            return new AssetDto()
            {
                ID = 1,
                AssetName = "Super computer - Changed",
                Department = "1",
                Country = "BRA",
                Email = "email@email.com",
                PurchaseDate = DateTimeOffset.UtcNow.Date,
                Broken = false
            };
        }
    }
}
