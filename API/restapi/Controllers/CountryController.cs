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
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var fields = metadataRepository.GetFields(countryName);

            if(fields == null)
            {
                logger.LogInformation($"Country {countryName} wasn't found");
                return NotFound();
            }

            return Ok(fields.Fields);
        }

        /*
        [HttpGet]
        [ProducesResponseType(typeof(Address), 200)]
        public IActionResult SearchAddresses([FromBody] Dictionary<string, string> query)  // FIXME- not frombody?
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var addressList = countryRepository.Search(query);
            return Ok(addressList);
        }
        */
    }
}
