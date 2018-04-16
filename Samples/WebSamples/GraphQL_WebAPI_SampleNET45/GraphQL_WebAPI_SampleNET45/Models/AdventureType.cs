using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class AdventureType : ObjectGraphType<Adventure>
    {
        public AdventureType()
        {
            Field(a => a.Name);
            Field(a => a.Description);
            Field(a => a.Cost);
        }
    }
}