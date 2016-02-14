using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Entities
{
    public class Brochure : Entity<int>
    {
        public Brochure()
        {

        }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }
    }
}
