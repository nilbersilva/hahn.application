using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface IAssetService
    {
        Task<int> CreateAsync(AssetDto asset);

        Task<bool> UpdateAsync(AssetDto asset);

        Task<AssetDto> GetAsync(int id);

        Task DeleteAsync(int id);

        IEnumerable<AssetDto> GetByPage(Expression<Func<Asset, bool>> where, int page = 1, int itemsPerPage = 10);

        Task<long> CountAsync(Expression<Func<Asset, bool>> where);
    }
}
