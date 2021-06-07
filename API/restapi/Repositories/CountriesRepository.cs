using System.Collections.Generic;
using System.Linq;
using restapi.Interfaces;
using restapi.Models;
using restapi.Helpers;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace restapi
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly MetaDataContext _context;
        private readonly IConfiguration configuration;

        public CountriesRepository(MetaDataContext ctx, IConfiguration config)
        {
            _context = ctx;
            configuration = config;
        }

        public List<Address> Search(string countryName, Dictionary<string, string> query)
        {
            // return null;
            // Mock:
            /*
            List<Address> retVal = new List<Address>();
            Dictionary<string, string> dict1 = new Dictionary<string, string>() { "Street": "TestStreet", "City": "TestCity", "State", "TestState" };
            retVal.Add(new Address(dict1));
            Dictionary<string, string> dict2 = new Dictionary<string, string>() { "Street": "AnotherTestStreet", "City": "AnotherTestCity", "State", "AnotherTestState" };
            retVal.Add(new Addres(dict2));
            return retVal;
            */
            List<Address> retVal = new List<Address>();

            // use escape characters for columns with spaces
            string queryString = "SELECT * FROM " + countryName + " WHERE \"";
            foreach(KeyValuePair<string, string> kvp in query)
            {
                queryString += kvp.Key + "\" == \"" + kvp.Value + "\"";
            }
            queryString += ";";
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try 
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<string, string> fieldsInAddress = new Dictionary<string, string>();
                        for(int i=0;i<reader.FieldCount;i++)
                        {
                            fieldsInAddress.Add(reader.GetName(i), reader.GetString(i));
                        }
                        Address address = new Address(fieldsInAddress);
                        retVal.Add(address);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return retVal;
        }

        public bool AddAddress(string countryName, Dictionary<string, string> query)
        {
            if(query.Count == 0)
                return false;
            // use escape characters for columns with spaces
            string queryString = "INSERT INTO \"" + countryName + "\" (";
            
            // build lists first so that the order matches - not sure if Dictionary iterates in the same order every time
            List<string> columnNames = new List<string>();
            List<string> columnValues = new List<string>();
            foreach(KeyValuePair<string, string> kvp in query)
            {
                columnNames.Add(kvp.Key);
                columnValues.Add(kvp.Value);
            }

            bool firstCol = true;
            foreach(string col in columnNames)
            {
                if(!firstCol)
                    queryString += ",";
                queryString += "\"" + col + "\"";
                firstCol = false;
            }
            queryString += ") VALUES (";

            bool firstVal = true;
            foreach(string val in columnValues)
            {
                if(!firstVal)
                    queryString += ",";
                queryString += "\'" + val + "\'";
                firstVal = false;
            }
            queryString += ");";

            int rowsAffected = _context.Database.ExecuteSqlRaw(queryString);
            if(rowsAffected == 1)
                return true;
            return false;
        }
    }
}