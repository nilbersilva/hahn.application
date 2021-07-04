using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class GetCountriesResponse
    {
        /// <summary>
        /// List of country data transfer object
        /// </summary>
        public IList<CountryDto> Countries { get; set; }

        public GetCountriesResponse()
        {
            Countries = new List<CountryDto>();
        }       
    }
}
