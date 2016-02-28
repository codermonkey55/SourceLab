using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSnacks.Data.EF.Entities;

namespace QuickSnacks.Data.EF.EntityConfigurations
{
    public class MenuConfiguration : EntityTypeConfiguration<Menu>
    {
        public MenuConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Id).IsRequired().HasColumnName("MenuId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.Description).IsOptional().HasMaxLength(250);

            //this.Property(p => p.CreateDate).IsRequired();
            //this.Property(p => p.EditDate).IsOptional();
            this.Map(x => x.MapInheritedProperties());

            this.HasRequired(x => x.Brochure).WithMany(x => x.Menus).Map(x => x.MapKey("BrochureId")).WillCascadeOnDelete(true);
            //this.HasMany(x => x.MenuItems).WithRequired(x => x.Menu).Map(x => x.MapKey("MenuId")).WillCascadeOnDelete(true);
        }
    }
}
