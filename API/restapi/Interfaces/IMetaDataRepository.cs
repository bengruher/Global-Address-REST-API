using restapi.Models;
using System.Collections.Generic;

namespace restapi.Interfaces
{
    public interface IMetaDataRepository
    {
        IEnumerable<string> GetCountries();             // gets all country names
        CountryFields GetFields(string CountryName);    // gets all fields (names and types) for given country

        // functions for adding metadata during reading of config file at startup
        void ReadConfig();
        // void AddCountry();
        // void AddField();
        // void AddFieldType();

        // functions for creating country tables from metadata configuation
        void GenCountryTables();
    }
}