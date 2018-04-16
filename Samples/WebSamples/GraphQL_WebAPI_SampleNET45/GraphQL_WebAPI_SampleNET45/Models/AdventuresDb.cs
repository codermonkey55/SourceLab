using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphQL_WebAPI_SampleNET45.Models
{
    public class AdventuresDb
    {
        private List<Adventure> _adventuresDb;
        public AdventuresDb()
        {
            _adventuresDb = new List<Adventure>
            {
                new Adventure("Luray Caverns", "Luray Caverns, originally called Luray Cave, is a commercial cave just west of Luray, Virginia, United States", 50),
                new Adventure("Skyline Caverns", "Skyline Caverns is a series of geologic caves and a tourist attraction located in Warren County, Virginia, one mile south of Front Royal.", 40),
                new Adventure("Harpers Ferry", "Harpers Ferry Adventure Center, formerly Butts Tubes / BTI Whitewater offers tubing, whitewater rafting, zipline, canoeing, kayaking, team building, fishing, hiking, camping and more, on the Shenandoah and Potomac rivers in the Tri-state area of West Virginia", 80),
            };
        }

        internal object GetAdventureByName(string name)
        {
            return _adventuresDb.FirstOrDefault(a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        internal IEnumerable<Adventure> GetAdventures()
        {
           return _adventuresDb;
        }
    }
}