using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.Entities
{
    public interface IAuditableEntity
    {
        DateTime CreateDate { get; set; }

        DateTime? EditDate { get; set; }
    }
}
