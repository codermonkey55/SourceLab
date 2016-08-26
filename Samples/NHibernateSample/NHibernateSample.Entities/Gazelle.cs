using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public class Gazelle : Animal<int>
    {
        public virtual int NumberofHorns { get; set; }

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
