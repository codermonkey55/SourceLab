namespace EFCoreSample.Database
{
    public interface IEntityConfigurations
    {
        IEntityConfigurations Add<TEntity, TEntityConfiguration>()
            where TEntity : class
            where TEntityConfiguration : IEntityBuilder<TEntity>, new();

        IEntityConfigurations Add<TEntityConfiguration>() where TEntityConfiguration : IEntityBuilder, new();

        IEntityConfigurations Add<TEntity>(IEntityBuilder<TEntity> ec) where TEntity : class;

        IEntityConfigurations Add(IEntityBuilder ec);

        void End();
    }
}
