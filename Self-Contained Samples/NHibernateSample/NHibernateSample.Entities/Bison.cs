using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public class Bison : Animal<Guid>
    {
        public virtual string FurThickness { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
