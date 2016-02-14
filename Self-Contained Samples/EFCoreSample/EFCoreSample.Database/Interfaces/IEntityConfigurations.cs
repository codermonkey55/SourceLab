using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Database
{
    public interface IEntityConfigurations
    {
        IEntityConfigurations Add<TEntity, TEntityConfiguration>()
            where TEntity : class
            where TEntityConfiguration : IEntityBuilder<TEntity>, new();

        IEntityConfigurations Add<TEntityConfiguration>() where TEntityConfiguration : IEntityBuilder, new();

        IEntityConfigurations Add(IEntityBuilder ec);

        void End();
    }
}
