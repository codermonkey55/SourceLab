using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public class Lion : Animal<int>
    {
        public virtual int TeethCount { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
