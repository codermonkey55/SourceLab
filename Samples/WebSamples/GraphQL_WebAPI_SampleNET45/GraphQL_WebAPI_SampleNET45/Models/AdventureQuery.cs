using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class AdventureQuery : ObjectGraphType<object>
    {
        public AdventureQuery(AdventuresDb data)
        {
            Name = "Query";

            Field<ListGraphType<AdventureType>>("adventures", resolve: context => data.GetAdventures());

            Field<AdventureType>(
                "adventure",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name", Description = "name of adventure center" }
                ),
                resolve: context => data.GetAdventureByName(context.GetArgument<string>("name"))
            );
        }
    }
}