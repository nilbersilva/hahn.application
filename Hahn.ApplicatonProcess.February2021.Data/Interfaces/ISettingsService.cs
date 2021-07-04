using System;
using System.Globalization;

namespace Hahn.ApplicatonProcess.February2021.Data.Interfaces
{
    public interface ISettingsService
    {
        Uri GetRestCountriesUri();
        string[] GetExcludeCountries();

        Uri GetTopLevelDomainsUri();
    }
}
