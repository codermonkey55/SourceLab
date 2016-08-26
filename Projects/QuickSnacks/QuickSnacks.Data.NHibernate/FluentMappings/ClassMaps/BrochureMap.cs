using FluentNHibernate;
using FluentNHibernate.Mapping;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps
{
    public class BrochureMap : BaseMaps.AuditMap<Brochure>
    {
        public BrochureMap()
        {
            this.Id(x => x.Id).Not.Nullable().Column("BrochureId").GeneratedBy.Native();
            this.Map(x => x.Title).Length(50).Not.Nullable();
            this.Map(x => x.Description).Length(250).Nullable();

            this.HasMany(b => b.Menus).LazyLoad()
                                      .AsSet()
                                      .KeyColumn("BrochureId")
                                      .ForeignKeyConstraintName("BrochureId")
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