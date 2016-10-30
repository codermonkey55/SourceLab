using QuickSnacks.Data.NHibernate.Entities.Base;

namespace QuickSnacks.Data.NHibernate.FluentMappings.BaseMaps
{
    public class AuditMap<TEntity> : CommonMap<TEntity> where TEntity : AuditableEntity
    {
        public AuditMap()
        {
            this.Map(x => x.CreateDate);

            this.Map(x => x.EditDate);

            this.Component(x => x.AuditInfo);
            this.Component(x => x.AuditInfo, mapper =>
            {
                mapper.Map(x => x.CreateDate);
                mapper.Map(x => x.EditDate);
            });
        }
    }
}
