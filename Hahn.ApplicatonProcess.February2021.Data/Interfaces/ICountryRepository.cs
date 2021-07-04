using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface ICountryRepository
    {
        Task<IList<CountryDto>> GetAllCountriesAsync(Uri uri);
    }
}
