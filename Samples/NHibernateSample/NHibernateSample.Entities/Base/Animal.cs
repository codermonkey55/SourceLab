using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public abstract class Animal<TKey> : Entity<TKey> where TKey : struct
    {
        public virtual bool HasTeeth { get; set; }
        public virtual bool HasFur { get; set; }
        public virtual bool HasHorns { get; set; }
    }
}
