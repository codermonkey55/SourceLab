using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.EF.Entities
{
    public class MenuItem : Entity<int>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual FingerItem FingerItem { get; set; }
    }
}
