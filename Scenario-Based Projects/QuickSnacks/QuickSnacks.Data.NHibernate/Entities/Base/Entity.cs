using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities.Base
{
    public abstract class Entity<TKey> : AuditableEntity where TKey : struct
    {
        public virtual TKey Id { get; protected set; }
    }
}
