using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps
{
    public class MenuItemMap : BaseMaps.AuditMap<MenuItem>
    {
        public MenuItemMap()
        {
            this.Id(x => x.Id).Not.Nullable().Column("MenuItemId").GeneratedBy.Native();
            this.Map(x => x.Name).Length(50).Not.Nullable();
            this.Map(x => x.Description).Length(250).Nullable();

            Join("", part =>
            {
                part.Table("");
            });

            this.References(p => p.Menu, "MenuId").Not.Nullable();

            this.References(p => p.FingerItem, "FingerItemId").Not.Nullable();

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