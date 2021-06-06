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
        private readonly ICountriesRepository countriesRepository;

        public CountryController(ILogger<CountryController> logger, IMetaDataRepository metadataRepository, ICountriesRepository countriesRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
            this.countriesRepository = countriesRepository ?? throw new ArgumentNullException(nameof(countriesRepository));
        }

        [HttpGet("{countryName}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        public IActionResult GetFields(string countryName)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var fields = metadataRepository.GetFields(countryName);

            if(fields == null)
            {
                logger.LogInformation($"Country {countryName} wasn't found");
                return NotFound();
            }

            return Ok(fields.Fields);
        }

        [HttpGet("search/{countryName}")]
        [ProducesResponseType(typeof(Address), 200)]
        public IActionResult SearchAddresses(string countryName, [FromQuery] Dictionary<string, string> query) 
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var addressList = countriesRepository.Search(countryName, query);
            return Ok(addressList);
        }

        /*
        public IActionResult SearchAddresses(string countryName, string keywords)
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            string[] keys = keywords.Split('&');
            var query = new Dictionary<string, string>();
            foreach(var key in keys)
            {
                string paramName = key.Split('=')[0];
                string paramValue = key.Split('=')[1];
                query.Add(paramName, paramValue);
            }
            var addressList = countriesRepository.Search(countryName, query);
            return Ok(addressList);
        }
        */

        [HttpPost("{countryName}")]
        public IActionResult AddAddress(string countryName, [FromBody] Dictionary<string, string> fields) // FIXME - not frombody?
        {
            return Ok(countryName);
        }
    }
}
