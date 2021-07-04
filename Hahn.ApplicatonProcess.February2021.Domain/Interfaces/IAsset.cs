using Hahn.ApplicatonProcess.February2021.Domain.Enums.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Interfaces
{
    public interface IAsset
    { 
        public string AssetName { get; set; }

        public enDepartment Department { get; set; }

        public string CountryOfDepartment { get; set; }

        public string EMailAdressOfDepartment { get; set; }

        public DateTimeOffset PurchaseDate { get; set; }

        public bool? Broken { get; set; }
    }
}
