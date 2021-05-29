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

        // public RootController(ILogger<TimesheetsController> logger, IMetaDataRepository metadataRepository)
        public RootController(IMetaDataRepository metadataRepository)
        {
            // this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        /*
        // GET api/values
        [Route("~/")]
        [HttpGet]
        [Produces(ContentTypes.Root)]
        [ProducesResponseType(typeof(IDictionary<ApplicationRelationship, object>), 200)]
        public IDictionary<ApplicationRelationship, object> Get()
        {
            return new Dictionary<ApplicationRelationship, object>()
            {
                {
                    ApplicationRelationship.Timesheets, new List<DocumentLink>()
                    {
                        new DocumentLink()
                        {
                            Method = Method.Get,
                            Type = ContentTypes.Timesheets,
                            Relationship = DocumentRelationship.Timesheets,
                            Reference = "/timesheets"
                        },
                        new DocumentLink()
                        {
                            Method = Method.Post,
                            Type = ContentTypes.Timesheets,
                            Relationship = DocumentRelationship.Timesheets,
                            Reference = "/timesheets"
                        }
                    } 
                },
                {
                    ApplicationRelationship.Employees, new List<DocumentLink>()
                    {
                        new DocumentLink()
                        {
                            Method = Method.Get,
                            Type = ContentTypes.Employees,
                            Relationship = DocumentRelationship.Employees,
                            Reference = "/employees"
                        },
                        new DocumentLink()
                        {
                            Method = Method.Post,
                            Type = ContentTypes.Employees,
                            Relationship = DocumentRelationship.Employees,
                            Reference = "/employees"
                        }
                    }
                },
                {
                    //
                    // remember, if we make a breaking change to this API
                    // we need to change the version number
                    //
                    ApplicationRelationship.Version, "0.1"
                }
            };
        }

        [Route("~/")]
        [HttpPost]
        [Produces(ContentTypes.Timesheet)]
        [ProducesResponseType(typeof(Timecard), 200)]
        public Timecard Create([FromBody] DocumentPerson person)
        {
            if (person == null)
            {
                return null;
            }

            logger.LogInformation($"Creating timesheet for {person}");

            var timecard = new Timecard(person.Id);

            var entered = new Entered() { Person = person.Id };

            timecard.Transitions.Add(new Transition(entered));

            timesheetRepository.Add(timecard);

            return timecard;
        }
        */
    }
}
