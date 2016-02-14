using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities.Base
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }

        public virtual Components.AuditInfo AuditInfo { get; set; }
    }
}
