using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities.Components
{
    public class AuditInfo
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }
    }
}
