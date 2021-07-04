using Hahn.ApplicatonProcess.February2021.Domain.Attributes;
using Hahn.ApplicatonProcess.February2021.Domain.Enums.Business;
using Hahn.ApplicatonProcess.February2021.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.Entities
{
    [Table("Assets")]
    public class Asset : IEntity, IAsset, IAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MatchParent("AssetName")]
        public string AssetName { get; set; }

        [Required]
        [MatchParent("Department")]
        public enDepartment Department { get; set; }

        [Required]
        [MatchParent("CountryOfDepartment")]
        public string CountryOfDepartment { get; set; }

        [Required]
        [MatchParent("EMailAdressOfDepartment")]
        public string EMailAdressOfDepartment { get; set; }

        [Required]
        [MatchParent("PurchaseDate")]
        public DateTimeOffset PurchaseDate { get; set; }

        [MatchParent("Broken")]
        public bool? Broken { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public string CreatByIPAddress { get; set; }

        public string ModifiedByIPAddress { get; set; }

        [Column(TypeName = "timestamp")]
        public byte[] VersionStamp { get; set; }
    }
}
