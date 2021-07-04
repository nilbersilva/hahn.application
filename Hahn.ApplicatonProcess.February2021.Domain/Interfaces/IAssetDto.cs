using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Interfaces
{
    public interface IAssetDto
    {
        string AssetName { get; set; }
        string Department { get; set; }
        string Country { get; set; }
        string Email { get; set; }
        DateTimeOffset PurchaseDate { get; set; }
        bool? Broken { get; set; }
    }
}
