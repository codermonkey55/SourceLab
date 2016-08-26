using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    interface IIdentifiableEntity<TId> where TId : struct
    {
        TId Id { get; }
    }
}
