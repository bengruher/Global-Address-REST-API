using System.Collections.Generic;
using System.Linq;
using LiteDB;
using restapi.Interfaces;
using restapi.Models;

namespace restapi
{
    public class MetaDataRepository : IMetaDataRepository
    {
        const string DATABASE_FILE = "filename=metadata.db;mode=exclusive";

        public IEnumerable<string> GetCountries() 
        {
            // MOCK:
            return new List<string>() { "USA" };
        }

        public CountryFields GetFields(string CountryName) 
        {
            // MOCK:
            Dictionary<string, string> mockFieldDict = new Dictionary<string, string>();
            mockFieldDict.Add("Street", "TEXT");
            mockFieldDict.Add("City", "TEXT");
            mockFieldDict.Add("State", "TEXT");
            return new CountryFields(CountryName, mockFieldDict);
        }

        /*
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