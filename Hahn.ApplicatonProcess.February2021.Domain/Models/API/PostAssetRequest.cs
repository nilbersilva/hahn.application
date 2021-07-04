using Hahn.ApplicatonProcess.February2021.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class PostAssetRequest : IAssetDto
    {
        public string AssetName { get;set; }
        public string Department { get;set; }
        public string Country { get;set; }
        public string Email { get;set; }
        public DateTimeOffset PurchaseDate { get;set; }
        public bool? Broken { get; set; }
    }
}
