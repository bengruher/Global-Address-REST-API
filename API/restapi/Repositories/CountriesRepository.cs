using System.Collections.Generic;
using System.Linq;
using restapi.Interfaces;
using restapi.Models;
using restapi.Helpers;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace restapi
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly MetaDataContext _context;

        public CountriesRepository(MetaDataContext ctx)
        {
            _context = ctx;
        }

        public List<Address> Search(Dictionary<string, string> query)
        {
            return null;
            
            // Mock:
            /*
            List<Address> retVal = new List<Address>();
            Dictionary<string, string> dict1 = new Dictionary<string, string>() { "Street": "TestStreet", "City": "TestCity", "State", "TestState" };
            retVal.Add(new Address(dict1));
            Dictionary<string, string> dict2 = new Dictionary<string, string>() { "Street": "AnotherTestStreet", "City": "AnotherTestCity", "State", "AnotherTestState" };
            retVal.Add(new Addres(dict2));
            return retVal;
            */
        }
    }
}