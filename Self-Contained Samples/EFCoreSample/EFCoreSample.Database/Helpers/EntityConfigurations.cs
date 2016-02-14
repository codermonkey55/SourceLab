using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Database
{
    public class EntityConfigurations : IEntityConfigurations
    {
        private ModelBuilder _mb;

        public EntityConfigurations(ModelBuilder mb)
        {
            _mb = mb;
        }

        public IEntityConfigurations Add<TEntity, TEntityBuilder>()
            where TEntity : class
            where TEntityBuilder : IEntityBuilder<TEntity>, new()
        {
            _mb.Entity<TEntity>(new TEntityBuilder().Build);

            return this;
        }

        public IEntityConfigurations Add<TEntityBuilder>() where TEntityBuilder : IEntityBuilder, new()
        {
            var eb = new TEntityBuilder();

            _mb.Entity(eb.EntityType, eb.Build);

            return this;
        }

        public IEntityConfigurations Add(IEntityBuilder eb)
        {
            if (eb == null) throw new ArgumentNullException("The object of type IEntityBuilder cannot be null.");

            _mb.Entity(eb.EntityType, eb.Build);

            return this;
        }

        public void End()
        {
            this._mb = null;
        }
    }
}
