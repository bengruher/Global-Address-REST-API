using System.Collections.Generic;
using System.Linq;
using LiteDB;
using restapi.Interfaces;
using restapi.Models;
using restapi.Helpers;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;

namespace restapi
{
    public class MetaDataContext : DbContext
    {
        public MetaDataContext() : base() { }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Fields> Fields { get; set; }
    }

    public class MetaDataRepository : IMetaDataRepository
    {
        // const string DATABASE_FILE = "filename=metadata.db;mode=exclusive";
        const string ADDRESS_CONFIG_FILE = "addressConfig.json";

        public IEnumerable<string> GetCountries() 
        {
            // MOCK:
            return new List<string>() { "USA", "Canada", "India", "Mexico" };

            /*
            using(var database = new LiteDatabase(DATABASE_FILE))
            {
                var countries = database.GetCollection<string>("Countries");
                return countries.All;
            }
            */
        }

        public CountryFields GetFields(string CountryName) 
        {
            // MOCK:
            Dictionary<string, string> mockFieldDict = new Dictionary<string, string>();
            mockFieldDict.Add("Street", "[0-9]{3}$");
            mockFieldDict.Add("City", "[0-9]{4}$");
            mockFieldDict.Add("State", "[0-9]{5}$");
            return new CountryFields(CountryName, mockFieldDict);

            /*
            Dictionary<string, Field> fieldDict = new Dictionary<string, Field>();

            using(var database = new LiteDatabase(DATABASE_FILE))
            {
                var CountryFields = database.GetCollection<Field>("CountryFields"); // FIXME
                var fields = CountryFields.Select(CountryName == CountryName); // FIXME
                foreach(field in fields) // FIXME
                {
                    fieldDict.Add(field.Name, field.field); // FIXME
                }
            }
            return new CountryFields(CountryName, fieldDict);
            */
        }

        public void ReadConfig()
        {
            using(Stream inputStream = File.Open(ADDRESS_CONFIG_FILE, FileMode.Open)) 
            {
                List<CountryFields> countryFormats = PublicJsonSerializer.Deserialize<List<CountryFields>>(inputStream);
                foreach(CountryFields format in countryFormats)
                {
                    // add to metadata database - need to populate all 3 tables
                    // 1. Add country Countries table
                    // AddCountry(format.CountryName);

                    // 2. Add country fields to CountryFields table
                    // AddField(format.CountryName, ) // FIXME - add parameters

                    // 3. Add field type to FieldTypes table if it doesn't already exist
                    // AddFieldType() // FIXME - add parameters and either put existence check here or in AddFieldType()
                }
            }
        }

        public void GenCountryTables()
        {
        }




        /*

        EXAMPLES FROM TIMECARD PROJECT

        public IEnumerable<Person> All
        {
            get
            {
                using (var database = new LiteDatabase(DATABASE_FILE))
                {
                    var employees = database.GetCollection<Person>("employees");

                    return employees.Find(Query.All()).ToList();
                }
            }
        }

        public Person Find(int id)
        {
            Person employee = null;

            using (var database = new LiteDatabase(DATABASE_FILE))
            {
                var employees = database.GetCollection<Person>("employees");

                employee = employees
                    .FindOne(t => t.EmployeeId == id);
            }

            return employee;
        }

        public void Add(Person employee)
        {
            using (var database = new LiteDatabase(DATABASE_FILE))
            {
                var employees = database.GetCollection<Person>("employees");

                employees.Insert(employee);
            }
        }

        public void Update(Person employee)
        {
            using (var database = new LiteDatabase(DATABASE_FILE))
            {
                var employees = database.GetCollection<Person>("employees");

                employees.Update(employee);
            }
        }

        public void Delete(int id)
        {
            using (var database = new LiteDatabase(DATABASE_FILE))
            {
                var employees = database.GetCollection<Person>("employees");

                employees.DeleteMany(t => t.EmployeeId == id);
            }
        }
        */
    }
}
