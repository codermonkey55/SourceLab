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

            //-> Analogous to Reference mapping method for fluent-nhibernate.
            ManyToOne(b => b.Brochure, mapping =>
            {
                mapping.Column("Brochure_Id");
                mapping.Unique(true);
                mapping.Class(typeof(Brochure));
                mapping.Cascade(Cascade.None);
                mapping.Lazy(LazyRelation.NoLazy); // or .NoProxy, .NoLazy
                mapping.Access(Accessor.Property);
            });

            OneToOne(x => x.Brochure, mapper =>
            {
                mapper.Cascade(Cascade.Persist);
                mapper.Constrained(true);
                mapper.Lazy(LazyRelation.Proxy); // or .NoProxy, .NoLazy
                mapper.PropertyReference(typeof(Brochure).GetPropertyOrFieldMatchingName(nameof(MenuItem.Menu)));
                mapper.OptimisticLock(true);
                mapper.Formula("arbitrary SQL expression");
                mapper.Access(Accessor.Field);
            });

            Set(e => e.MenuItems, mapper =>
            {
                mapper.Key(k => k.Column("MenuId"));
                mapper.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
                mapper.Inverse(false);
                mapper.Table("MenuItem");
            },
            relation =>
            {
                relation.OneToMany(mapping => mapping.Class(typeof(MenuItem)));
            });

            Set(e => e.MenuItems, mapper =>
            {
                mapper.Key(k => k.Column("Menu_Id"));
                mapper.Table("Employee_Community");
                mapper.Cascade(Cascade.All);
            },
            relation =>
            {
                relation.ManyToMany(mtm =>
                {
                    mtm.Class(typeof(MenuItem));
                    mtm.Column("MenuItem_Id");
                });
            });
        }
    }
}
