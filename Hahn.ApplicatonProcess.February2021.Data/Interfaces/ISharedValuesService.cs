using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface ISharedValuesService
    {
        Task<IList<CountryDto>> GetAllCountriesAsync();
        Task<IList<DepartmentDto>> GetAllDepartmentsAsync();
        Task<IList<string>> GetAllTopLevelDomainsAsync();

        void PutAllCountriesInCache();
        void PutAllAllTopLevelDomainsInCache();
    }
}
