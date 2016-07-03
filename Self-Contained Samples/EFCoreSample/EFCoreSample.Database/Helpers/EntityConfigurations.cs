using System;
using Microsoft.EntityFrameworkCore;

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

        public IEntityConfigurations Add<TEntity>(IEntityBuilder<TEntity> eb)
    where TEntity : class
        {
            _mb.Entity<TEntity>(eb.Build);

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
