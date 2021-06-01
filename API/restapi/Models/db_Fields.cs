using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restapi.Models
{
    public class Fields
    {
        [Key]
        public string CountryName { get; set; }
        [Key]
        public string FieldName { get; set; }
        public string TypeString { get; set; }
    }
}