using ODACSample.EF.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ODACSample.EF.Mappings
{
	internal class CategoryConfiguration : EntityTypeConfiguration<Category>
	{
		/// <summary>
		/// For Mapping Oracle Sequences see reference link below...
		/// <see cref="http://stackoverflow.com/questions/13077810/oracle-sequences-entity-framework-and-getting-nextval-in-an-mvc3-view"/>
		/// </summary>
		internal CategoryConfiguration()
		{
			this.ToTable("NW.CATEGORIES");
			this.HasKey(x => x.CategoryId);

			this.Property(x => x.CategoryId).IsRequired().HasColumnName("CATEGORY_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			this.Property(x => x.CategoryName).HasColumnName("CATEGORY_NAME");
			this.Property(x => x.Description).HasColumnName("DESCRIPTION");
			this.Property(x => x.CategoryId).HasColumnName("CATEGORY_ID");
			this.Property(x => x.Picture).HasColumnName("PICTURE");

			this.HasMany(x => x.Products).WithRequired(x => x.Category).WillCascadeOnDelete(true);

		}
	}
}
