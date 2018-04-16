using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class AdventureSchema : Schema
    {
        public AdventureSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<AdventureQuery>();
        }
    }
}