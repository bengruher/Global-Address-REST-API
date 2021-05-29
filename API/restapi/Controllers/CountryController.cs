using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using restapi.Interfaces;
using restapi.Models;

namespace restapi.Controllers
{
    // [Route("[controller]")]
    public class CountryController : Controller
    {
        private readonly ILogger logger;
        private readonly IMetaDataRepository metadataRepository;

        public CountryController(ILogger<CountryController> logger, IMetaDataRepository metadataRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
        }

        [HttpGet("{countryName}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        public IActionResult GetFields(string countryName)
        {
            var fields = metadataRepository.GetFields(countryName);

            if(fields == null)
            {
                logger.LogInformation($"Country {countryName} wasn't found");
                return NotFound();
            }

            return Ok(fields.Fields);
        }

        /*
        [HttpGet("{employeeId:int}")]
        [Produces(ContentTypes.Employee)]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOne(int employeeId)
        {
            Person employee = employeeRepository.Find(employeeId);

            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                logger.LogInformation($"Employee {employeeId} wasn't found");
                return NotFound();
            }
        }

        [HttpPost]
        [Produces(ContentTypes.Employee)]
        [ProducesResponseType(typeof(Person), 200)]
        public IActionResult Create([FromBody] Person employee)
        {
            logger.LogInformation($"Looking for employee {employee.EmployeeId}");
            try         // try to fetch employee if they already exist
            {
                var currentEmployee = employeeRepository.Find(employee.EmployeeId);
                if(currentEmployee != null) 
                {
                    return StatusCode(409, new EmployeeAlreadyExists() { });
                }
            }
            catch {}    // employee does not currently exist

            bool firstEmployee = false;
            if(employeeRepository.All.Count() == 0)  // first employee being created cannot already have a manager
            {   
                firstEmployee = true;
            }

            logger.LogInformation($"Looking for manager with id {employee.ManagerId}");
            try         // try to fetch manager if they already exist
            {
                var manager = employeeRepository.Find(employee.ManagerId);
                if(manager == null && firstEmployee == false) 
                {
                    return StatusCode(409, new NoManagerFound() { });
                }
                if(manager != null && employee.EmployeeId == manager.EmployeeId)
                {
                    return StatusCode(409, new InvalidEmployeeRegistration() { });
                }
            }
            catch       // manager does not exist (this might be okay if employee being added is the first employee)
            {
                if(firstEmployee == false) 
                {
                    return StatusCode(409, new NoManagerFound() { });
                }
            }

            employeeRepository.Add(employee);

            return Ok(employee);
        }

        [HttpGet("{employeeId:int}/reports")]
        [Produces(ContentTypes.Employees)]
        [ProducesResponseType(typeof(IList<Person>), 200)]
        [ProducesResponseType(404)]
        [SuppressMessage("Usage", "CA1801")]
        [SuppressMessage("Usage", "IDE0060")]
        public IActionResult GetReports(int employeeId)
        {
            logger.LogInformation($"Attempt to get reports for {employeeId}");
            return StatusCode(501, "Function not implemented");
        }

        [HttpGet("{employeeId:int}/organization")]
        [Produces(ContentTypes.Employees)]
        [ProducesResponseType(typeof(IList<Person>), 200)]
        [ProducesResponseType(404)]
        [SuppressMessage("Usage", "CA1801")]
        [SuppressMessage("Usage", "IDE0060")]
        public IActionResult GetOrganization(int employeeId)
        {
            logger.LogInformation($"Attempt to get organization for {employeeId}");
            return StatusCode(501, "Function not implemented");
        }

        [HttpGet("{employeeId:int}/manager")]
        [Produces(ContentTypes.Employees)]
        [ProducesResponseType(typeof(IList<Person>), 200)]
        [ProducesResponseType(404)]
        [SuppressMessage("Usage", "CA1801")]
        [SuppressMessage("Usage", "IDE0060")]
        public IActionResult GetManager(int employeeId)
        {
            logger.LogInformation($"Attempt to get manager for {employeeId}");
            return StatusCode(501, "Function not implemented");
        }
        */
    }
}
