using Hahn.ApplicatonProcess.February2021.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.DTO
{
    public class AssetDto : IAssetDto, IIdentified
    {
        /// <summary>
        /// The Asset unique identifier, maps to Asset entity, Id property
        /// </summary>
        /// <example>1</example>
        public int? ID { get; set; }

        /// <summary>
        /// The name of the Asset, maps to Asset entity, AssetName property
        /// </summary>
        /// <example>Super computer</example>
        public string AssetName { get; set; }

        /// <summary>
        /// The Department of the Asset, maps to Asset entity, Department property
        /// </summary>
        /// <example>1</example>
        public string Department { get; set; }

        /// <summary>
        /// The Asset department country, maps to Asset entity, CountryOfDepartment property
        /// </summary>
        /// <example>BRA</example>
        public string Country { get; set; }

        /// <summary>
        /// The Asset department email, maps to Asset entity, EMailAdressOfDepartment property
        /// </summary>
        /// <example>email@email.com</example>
        public string Email { get; set; }

        /// <summary>
        /// The Asset purchase date, maps to Asset entity, PurchaseDate property
        /// </summary>
        /// <example>2021-07-04T00:00:00-03:00</example>
        public DateTimeOffset PurchaseDate { get; set; }

        /// <summary>
        /// Indicates if asset is broken, if null defaults to false
        /// </summary>
        /// <example>false</example>
        public bool? Broken { get; set; }
    }
}
