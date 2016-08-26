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
    public class MenuItemConfiguration : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Id).IsRequired().HasColumnName("MenuItemId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.Description).IsOptional().HasMaxLength(250);

            //this.Property(p => p.CreateDate).IsRequired();
            //this.Property(p => p.EditDate).IsOptional();
            this.Map(x => x.MapInheritedProperties());

            this.HasRequired(x => x.Menu).WithMany(x => x.MenuItems).Map(x => x.MapKey("MenuId")).WillCascadeOnDelete(true);
            this.HasRequired(x => x.FingerItem).WithMany(x => x.MenuItems).Map(x => x.MapKey("FingerItemId")).WillCascadeOnDelete(true);
        }
    }
}
