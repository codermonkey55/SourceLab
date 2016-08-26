using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreSample.Database
{
    public interface IEntityBuilder<TEntity> : IEntityBuilder where TEntity : class
    {
        void Build(EntityTypeBuilder<TEntity> etb);
    }
}
