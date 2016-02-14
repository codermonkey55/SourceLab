using FluentNHibernate;
using FluentNHibernate.Mapping;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps
{
    public class FingerItemMap : BaseMaps.AuditMap<FingerItem>
    {
        public FingerItemMap()
        {
            this.Id(x => x.Id).Not.Nullable().Column("FingerItemId").GeneratedBy.Native();
            this.Map(x => x.Name).Length(50).Not.Nullable();
            this.Map(x => x.Description).Length(250).Nullable();

            this.HasMany(b => b.MenuItems).Not.LazyLoad()
                                          .AsSet()
                                          .KeyColumn("FingerItemId")
                                          .Cascade.DeleteOrphan();

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