using FluentValidation;
using FluentValidation.Results;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface IValidationService
    {
        ValidationResult Validate<U, V>(U item) where V : AbstractValidator<WithMemoryCache<AssetDto>>, new();
    }
}
