using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.Validation
{
    public class ValidationMessage
    {
        public ValidationMessage()
        {
            Property = String.Empty;
            Message = String.Empty;
        }

        ///<summary>
        /// Property validated 
        ///</summary>
        ///<example>assetName</example>
        public string Property { get; set; }

        ///<summary>
        /// Description what's wrong
        ///</summary>
        ///<example>assetName required</example>
        public string Message { get; set; }
    }
}
