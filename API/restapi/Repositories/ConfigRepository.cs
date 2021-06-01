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
    public class ConfigRepository // FIXME -add interface
    {
        static string ADDRESS_CONFIG_FILE = "addressConfig.json";
        private static MetaDataContext _context;
        private static MetaDataRepository metaRepo;

        public ConfigRepository(MetaDataContext ctx, MetaDataRepository mR)
        {
            _context = ctx;
            metaRepo = mR;
        }

        public static void ReadConfig()
        {
            using(Stream inputStream = File.Open(ADDRESS_CONFIG_FILE, FileMode.Open)) 
            {
                List<CountryFields> countryFormats = PublicJsonSerializer.Deserialize<List<CountryFields>>(inputStream);
                foreach(CountryFields format in countryFormats)
                {
                    // 1. Add country Countries table
                    metaRepo.AddCountry(format.CountryName);

                    // 2. Add country fields to CountryFields table
                    foreach(KeyValuePair<string, string> field in format.Fields)
                    {
                        metaRepo.AddField(format.CountryName, field.Key, field.Value);
                    }
                }
            }
        }

        public static void AddCountry(string CountryName)
        {
            _context.Add(new Countries { CountryName = CountryName });
            _context.SaveChanges();
        }

        public static void AddField(string CountryName, string FieldName, string TypeString)
        {
            _context.Add(new Fields { CountryName = CountryName, FieldName = FieldName, TypeString = TypeString });
            _context.SaveChanges();
        }

        public static void GenCountryTables()
        {
            // get countries
            var countries = metaRepo.GetCountries();

            // foreach country, replace country in query string and get fields for that country
            foreach(string country in countries)
            {
                string queryString = "CREATE TABLE ";
                queryString += country + "(";
                
                // get fields
                var countryFields = metaRepo.GetFields(country);

                bool firstCol = true; // do not add comma before first column

                // foreach field, add column to query string with string/TEXT type
                foreach(KeyValuePair<string, string> field in countryFields.Fields)
                {
                    if(!firstCol)
                        queryString += ", ";
                    queryString += field.Key + " varchar(255)"; // FIXME - NULLABLE? 
                    firstCol = false;
                }
                queryString += ");"; 
                
                // execute query
                // _context.Database.CreateIfNotExists();
                _context.Database.ExecuteSqlRaw(queryString);
            }
        }
    }
}
