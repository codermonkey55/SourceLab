using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Data.NHibernate.LoquaciousMappings
{
    public sealed class MenuMapping : ClassMapping<Menu>
    {
        public MenuMapping()
        {
            Id(prop => prop.Id, mapper =>
            {
                mapper.Generator(Generators.HighLow);
                mapper.Column("Id");
                mapper.Length(10);
                mapper.UnsavedValue(0);
            });

            Property(prop => prop.Name, mapper =>
            {
                mapper.Column("Name");
                mapper.Type(NHibernateUtil.String);
                mapper.Length(10);
                mapper.NotNullable(true);
                mapper.Lazy(true);
                mapper.Unique(true);
            });

            Set(e => e.MenuItems, mapper =>
            {
                mapper.Key(k => k.Column("MenuId"));
                mapper.Cascade(Cascade.All);
                mapper.Inverse(false);
                mapper.Table("MenuItem");
            },
            relation =>
            {
                relation.OneToMany(mapping => mapping.Class(typeof(MenuItem)));
            });
        }
    }
}
