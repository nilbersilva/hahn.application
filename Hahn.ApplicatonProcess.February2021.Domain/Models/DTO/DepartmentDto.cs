using Hahn.ApplicatonProcess.February2021.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.DTO
{
    public class DepartmentDto : IIdentified
    {
        /// <summary>
        /// The department identifier, maps to enDepartment Enum
        /// </summary>
        /// <example>1</example>
        public int? ID { get; set; }

        /// <summary>
        /// The department translation key, maps to Department enum
        /// </summary>
        /// <example>HQ</example>
        public string DepartmentName { get; set; }
    }
}
