using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities
{
    public class Brochure : Base.Entity<Int32>
    {
        public Brochure()
        {
            Menus = new HashSet<Menu>();
        }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual ISet<Menu> Menus { get; protected set; }

        public virtual void AddMenu(Menu menu)
        {
            menu.Brochure = this;

            this.Menus.Add(menu);
        }
    }
}
