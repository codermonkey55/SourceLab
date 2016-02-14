using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Entities
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }
    }
}
