using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.Entities
{
    public class Entity<TKey> : IAuditableEntity where TKey : struct
    {
        public TKey Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EditDate { get; set; }
    }
}
