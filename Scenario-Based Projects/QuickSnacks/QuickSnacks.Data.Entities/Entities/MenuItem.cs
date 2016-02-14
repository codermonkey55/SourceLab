using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.Entities
{
    public class MenuItem : Entity<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public Menu Menu { get; set; }

        public FingerItem FingerItem { get; set; }
    }
}
