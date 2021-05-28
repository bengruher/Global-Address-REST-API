using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using restapi.Helpers;

namespace restapi.Models 
{
    public class CountryFields 
    {
        public string CountryName; 
        public Dictionary<string, string> Fields;    // key: field name, value: field type/format

        public CountryFields(string countryName, Dictionary<string, string> fields)
        {
            CountryName = countryName;
            Fields = new Dictionary<string, string>(fields);
        }

        public CountryFields(string countryName)
        {
            CountryName = countryName;
            Fields = new Dictionary<string, string>();
        }

        public CountryFields()
        {
            Fields = new Dictionary<string, string>();
        }
    }
}