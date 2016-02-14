using Microsoft.Data.Entity.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Database
{
    public interface IEntityBuilder<TEntity> : IEntityBuilder where TEntity : class
    {
        void Build(EntityTypeBuilder<TEntity> etb);
    }
}
