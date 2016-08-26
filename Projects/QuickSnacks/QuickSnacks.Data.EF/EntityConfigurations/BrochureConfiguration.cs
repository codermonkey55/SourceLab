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
    public class BrochureConfiguration : EntityTypeConfiguration<Brochure>
    {
        public BrochureConfiguration()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Id).IsRequired().HasColumnName("BrochureId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Title).IsRequired().HasMaxLength(50);
            this.Property(x => x.Description).IsOptional().HasMaxLength(250);

            //this.Property(p => p.CreateDate).IsRequired();
            //this.Property(p => p.EditDate).IsOptional();
            this.Map(x => x.MapInheritedProperties());

            //this.HasMany(x => x.Menus).WithRequired(x => x.Brochure).Map(x => x.MapKey("BrochureId")).WillCascadeOnDelete(true);
        }
    }
}
