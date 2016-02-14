using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public class Habitat : Entity<int>
    {
        public virtual float Area { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
