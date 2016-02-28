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
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey(k => k.Id);

            this.Property(x => x.Id).HasColumnName("UserId").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            this.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            this.Property(p => p.FullName).IsOptional().HasMaxLength(101).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            //this.Property(p => p.CreateDate).IsRequired();
            //this.Property(p => p.EditDate).IsOptional();
            this.Map(m => m.MapInheritedProperties());

            this.HasOptional(x => x.ActiveToken).WithMany(x => x.Users).Map(x => x.MapKey("ActiveTokenId")).WillCascadeOnDelete(true);
            //this.HasMany(x => x.Tokens).WithRequired(x => x.User).Map(x => x.MapKey("UserId")).WillCascadeOnDelete(true);
        }
    }
}
