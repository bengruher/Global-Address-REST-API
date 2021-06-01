using restapi.Models;
using System.Collections.Generic;

namespace restapi.Interfaces
{
    public interface ICountriesRepository
    {
        List<Address> Search(Dictionary<string, string> query);
    }
}