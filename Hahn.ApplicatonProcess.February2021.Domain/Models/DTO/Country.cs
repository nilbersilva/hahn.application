using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.DTO
{
    public class CountryDto
    {
        /// <summary>
        /// The country name
        /// </summary>
        /// <example>Brazil</example>
        public string Name { get; set; }

        /// <summary>
        /// The country 3 letter code in upper case
        /// </summary>
        /// <example>BRA</example>
        public string Alpha3Code { get; set; }

        private IDictionary<string, string> _translations;
        public IDictionary<string, string> Translations
        {
            get
            {
                if (!_translations.ContainsKey("en"))
                {
                    _translations.Add("en", Name);
                }
                return _translations;
            }
            set
            {
                _translations = value;
            }
        }

        public CountryDto()
        {
            Alpha3Code = string.Empty;
            Name = string.Empty;
            _translations = new Dictionary<string, string>();
        }
    }
}
