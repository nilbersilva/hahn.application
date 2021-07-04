using Hahn.ApplicatonProcess.February2021.Domain.Interfaces;
using System;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class PutAssetRequest : IIdentified, IAssetDto
    {
        public int? ID { get; set; }
        public string AssetName { get; set; }
        public string Department { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public bool? Broken { get; set; }
    }
}
