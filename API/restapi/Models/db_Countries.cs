using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace restapi.Models
{
    public class Countries
    {
        [Key]
        public int ID { get; set; }
        public string CountryName { get; set; }
    }
}