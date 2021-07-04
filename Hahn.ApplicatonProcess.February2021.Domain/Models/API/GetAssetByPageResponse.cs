using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class GetAssetByPageResponse
    {
        public long TotalItems { get; set; }
        public long TotalPages { get; set; }

        public IEnumerable<AssetDto> Assets { get; set; }
    }
}
