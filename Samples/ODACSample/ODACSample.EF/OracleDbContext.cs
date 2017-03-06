using ODACSample.EF.Entities;
using ODACSample.EF.Mappings;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ODACSample.EF
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext() : base("name=OracleDbContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            //Disable initializer
            Database.SetInitializer<OracleDbContext>(null);

            Database.Log = sql_Statement => Console.WriteLine(sql_Statement);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("NW");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
