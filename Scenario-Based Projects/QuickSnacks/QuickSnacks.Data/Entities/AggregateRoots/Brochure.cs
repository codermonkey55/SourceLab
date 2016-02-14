using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.EF.Entities
{
    public class Brochure : Entity<int>
    {
        public Brochure()
        {
            Menus = new HashSet<Menu>();
        }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<Menu> Menus { get; protected set; }

        public void AddMenu(Menu menu)
        {
            this.Menus.Add(menu);
        }
    }
}
