using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Entities
{
    public class Friend : Entity<Guid>
    {
        public int Age { get; set; }
    }
}
