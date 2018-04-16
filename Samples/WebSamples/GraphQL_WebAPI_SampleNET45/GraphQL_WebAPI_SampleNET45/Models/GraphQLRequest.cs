using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class GraphQLRequest
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public Newtonsoft.Json.Linq.JObject Variables { get; set; }
    }
}