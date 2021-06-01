using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using restapi.Helpers;

namespace restapi.Models 
{
    public class Address 
    {
        public Dictionary<string, string> Fields = new Dictionary<string, string>();    // key: field name, value: field value

        public Address(Dictionary<string, string> fields)
        {
            Fields = fields;
        }
    }
}