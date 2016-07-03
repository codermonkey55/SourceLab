using System;

namespace EFCoreSample.Entities
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? EditDate { get; set; }
    }
}
