using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.API
{
    public class GetTopLevelDomainsResponse
    {
        /// <summary>
        /// List of top level domains
        /// </summary>
        /// <example>[".com",".net"]</example>
        public IList<String> TopLevelDomains { get; set; }

        public GetTopLevelDomainsResponse()
        {
            TopLevelDomains = new List<string>();
        }     
    }
}
