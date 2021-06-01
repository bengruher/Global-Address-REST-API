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
    public class MetaDataRepository : IMetaDataRepository
    {
        const string ADDRESS_CONFIG_FILE = "addressConfig.json";
        private readonly MetaDataContext _context;

        public MetaDataRepository(MetaDataContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<string> GetCountries() 
        {
            // MOCK:
            // return new List<string>() { "USA", "Canada", "India", "Mexico" };

            var countries = _context.Countries.Select(c => c.CountryName);
            return countries;
        }

        public CountryFields GetFields(string CountryName) 
        {
            // MOCK:
            /*
            Dictionary<string, string> mockFieldDict = new Dictionary<string, string>();
            mockFieldDict.Add("Street", "[0-9]{3}$");
            mockFieldDict.Add("City", "[0-9]{4}$");
            mockFieldDict.Add("State", "[0-9]{5}$");
            return new CountryFields(CountryName, mockFieldDict);
            */

            Dictionary<string, string> fieldDict = new Dictionary<string, string>();

            var fields = _context.Fields.Where(c => c.CountryName == CountryName);
            foreach(Fields field in fields)
            {
                fieldDict.Add(field.FieldName, field.TypeString);
            }

            return new CountryFields(CountryName, fieldDict);
        }

        public void ReadConfig()
        {
            ClearTable("Countries");
            ClearTable("Fields");
            using(Stream inputStream = File.Open(ADDRESS_CONFIG_FILE, FileMode.Open)) 
            {
                List<CountryFields> countryFormats = PublicJsonSerializer.Deserialize<List<CountryFields>>(inputStream);
                foreach(CountryFields format in countryFormats)
                {
                    // 1. Add country Countries table
                    AddCountry(format.CountryName);

                    // 2. Add country fields to CountryFields table
                    foreach(KeyValuePair<string, string> field in format.Fields)
                    {
                        AddField(format.CountryName, field.Key, field.Value);
                    }
                }
            }
        }

        public void AddCountry(string CountryName)
        {
            _context.Add(new Countries { CountryName = CountryName });
            _context.SaveChanges();
        }

        public void AddField(string CountryName, string FieldName, string TypeString)
        {
            _context.Add(new Fields { CountryName = CountryName, FieldName = FieldName, TypeString = TypeString });
            _context.SaveChanges();
        }

        public void GenCountryTables()
        {
            // get countries
            var countries = GetCountries();

            // foreach country, replace country in query string and get fields for that country
            foreach(string country in countries)
            {
                // drop tables if they exist to recreate them in case columns have changed
                string dropString = "DROP TABLE IF EXISTS \"" + country + "\";";
                _context.Database.ExecuteSqlRaw(dropString);
                
                string queryString = "CREATE TABLE \"";
                queryString += country + "\"(";
                
                // get fields
                var countryFields = GetFields(country);

                bool firstCol = true; // do not add comma before first column

                // foreach field, add column to query string with string/TEXT type
                foreach(KeyValuePair<string, string> field in countryFields.Fields)
                {
                    if(!firstCol)
                        queryString += ", ";
                    queryString += "\"" + field.Key + "\" varchar(255)"; // FIXME - NULLABLE? 
                    firstCol = false;
                }
                queryString += ");"; 
                
                // execute query
                // _context.Database.CreateIfNotExists();
                _context.Database.ExecuteSqlRaw(queryString);
            }
        }
        public void ClearTable(string tableName)
        {
            _context.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
        }
    }
}
