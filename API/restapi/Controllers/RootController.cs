using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using restapi.Models;
using Microsoft.Extensions.Logging;
using restapi.Interfaces;
using System;
using System.Linq;

namespace restapi.Controllers
{
    public class RootController : Controller
    {
        private readonly IMetaDataRepository metadataRepository;
        private readonly ILogger logger;

        public RootController(ILogger<RootController> logger, IMetaDataRepository metadataRepository)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.metadataRepository = metadataRepository ?? throw new ArgumentNullException(nameof(metadataRepository));
        }

        // GET countries
        [Route("~/")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IEnumerable<string> GetCountries()
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var output = metadataRepository
                .GetCountries()
                .OrderBy(c => c);
            return output;
        }
    }
}
