using restapi.Models;
using System.Collections.Generic;

namespace restapi.Interfaces
{
    public interface ICountriesRepository
    {
        List<Address> Search(string countryName, Dictionary<string, string> query);
    }
}