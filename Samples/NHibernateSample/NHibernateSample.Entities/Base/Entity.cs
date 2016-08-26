using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public abstract class Entity<TKey> : IdentifiableEntity<TKey> where TKey : struct
    {

    }
}
