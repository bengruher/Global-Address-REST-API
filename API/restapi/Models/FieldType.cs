using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using restapi.Helpers;

namespace restapi.Models 
{
    public class FieldType 
    {
        public string fieldType; // FIXME

        public FieldType(string fieldType)
        {
            fieldType = fieldType; // FIXME
        }
    }
}