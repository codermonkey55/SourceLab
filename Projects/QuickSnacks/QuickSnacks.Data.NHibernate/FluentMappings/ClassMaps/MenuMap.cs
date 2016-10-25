using NHibernate.Persister.Entity;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps
{
    public class MenuMap : BaseMaps.AuditMap<Menu>
    {
        public MenuMap()
        {
            this.LazyLoad();
            this.Polymorphism.Implicit();
            this.Cache.IncludeNonLazy();
            this.SchemaAction.Validate();
            this.OptimisticLock.Dirty();
            this.HibernateMapping.DefaultLazy();

            this.Persister(typeof(IEntityPersister).Name);

            this.Id(x => x.Id).Not.Nullable().Column("MenuId").GeneratedBy.Native();
            this.Map(x => x.Name).Length(50).Not.Nullable();
            this.Map(x => x.Description).Length(250).Nullable();

            this.References(p => p.Brochure, "BrochureId").ForeignKey("BrochureId").Column("BrochureId").Not.Nullable();

            this.HasMany(b => b.MenuItems).Not.LazyLoad()
                                          .AsSet()
                                          .KeyColumn("MenuId")
                                          .Cascade.All();

            //this.DynamicInsert();
            //this.DynamicUpdate();

            //this.Map(x => x.CreateDate);
            //this.Map(x => x.EditDate);

            //this.Polymorphism.Explicit();
            //this.ImportType<BrochureDto>();
            //this.Component(x => x.AuditInfo);
        }
    }
}