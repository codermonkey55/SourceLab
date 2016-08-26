using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public abstract class IdentifiableEntity<TId> : AuditableEntity, IIdentifiableEntity<TId> where TId : struct
    {
        public virtual TId Id { get; set; }
    }
}
