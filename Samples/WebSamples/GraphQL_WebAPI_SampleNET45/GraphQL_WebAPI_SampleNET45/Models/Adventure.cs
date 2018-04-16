using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class Adventure
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public Adventure(string name, string description, int cost)
        {
            Name = name;
            Description = description;
            Cost = cost;
        }
    }
}