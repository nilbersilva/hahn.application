using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class GetDepartmentsResponse
    {
        /// <summary>
        /// List of department data transfer object
        /// </summary>
        public IList<DepartmentDto> Departments { get; set; }

        public GetDepartmentsResponse()
        {
            Departments = new List<DepartmentDto>();
        }
    }
}
