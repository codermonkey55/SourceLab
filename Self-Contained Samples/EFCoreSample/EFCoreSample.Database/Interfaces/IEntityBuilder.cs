using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Database
{
    public interface IEntityBuilder
    {
        Type EntityType { get; }

        void Build(EntityTypeBuilder etb);
    }
}
