using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.EF.Entities
{
    public class Menu : Entity<int>
    {
        public Menu()
        {
            MenuItems = new HashSet<MenuItem>();
        }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Brochure Brochure { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; protected set; }

        public void AddMenuItem(MenuItem menuItem)
        {
            this.MenuItems.Add(menuItem);
        }
    }
}
