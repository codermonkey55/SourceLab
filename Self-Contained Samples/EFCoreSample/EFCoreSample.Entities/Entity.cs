using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Entities
{
    public class Entity<TKey> : AuditableEntity where TKey : struct
    {
        public virtual TKey Id { get; protected set; }
    }
}
