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
    public class TokenConfiguration : EntityTypeConfiguration<Token>
    {
        public TokenConfiguration()
        {
            this.HasKey<int>(k => k.Id);

            this.Property(p => p.Id).IsRequired().HasColumnName("TokenId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.AuthKey).IsRequired();

            //this.Property(p => p.CreateDate).IsRequired();
            //this.Property(p => p.EditDate).IsOptional();
            this.Map(m => m.MapInheritedProperties());

            this.HasRequired(r => r.User).WithMany(m => m.Tokens).Map(x => x.MapKey("UserId")).WillCascadeOnDelete(true);
            //this.HasMany(x => x.Users).WithOptional(x => x.ActiveToken).Map(x => x.MapKey("ActiveTokenId")).WillCascadeOnDelete(true);
        }
    }
}
