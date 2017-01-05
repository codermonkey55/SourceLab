using ODACSample.EF.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ODACSample.EF.Mappings
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            this.ToTable("NW.PRODUCTS");
            this.HasKey(x => x.ProductId);

            this.Property(x => x.ProductId).IsRequired().HasColumnName("PRODUCT_ID").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(x => x.ProductName).HasColumnName("PRODUCT_NAME");
            this.Property(x => x.SupplierId).HasColumnName("SUPPLIER_ID");
            this.Property(x => x.CategoryId).HasColumnName("CATEGORY_ID");
            this.Property(x => x.QuantityPerUnit).HasColumnName("QUANTITY_PER_UNIT");
            this.Property(x => x.UnitPrice).HasColumnName("UNIT_PRICE");
            this.Property(x => x.UnitsInStock).HasColumnName("UNITS_IN_STOCK");
            this.Property(x => x.UnitsOnOrder).HasColumnName("UNITS_ON_ORDER");
            this.Property(x => x.ReorderLevel).HasColumnName("REORDER_LEVEL");
            this.Property(x => x.Discontinued).HasColumnName("DISCONTINUED");

            this.HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(true);
        }
    }
}
