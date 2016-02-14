using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities
{
    public class FingerItem : Base.Entity<Int32>
    {
        public FingerItem()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ISet<MenuItem> MenuItems { get; protected set; }
    }
}
