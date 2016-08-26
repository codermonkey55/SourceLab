using EFCoreSample.Entities;
using EFCoreSample.Database.Extensions;
using System;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreSample.Database
{
    public class QuickSnacksDbContext : DbContext
    {
        private static bool _created = false;

        private string connectionString = ConfigurationManager.ConnectionStrings["EFCoreSampleDb"]?.ConnectionString;

        private string defaultConnectionString = @"Data Source=.\SQL14_Local;Initial Catalog=EFCoreSampleDb;Persist Security Info=True;User ID=sa;Password=AccessSQL14Enterprise310;";


        public QuickSnacksDbContext()
        {
            if (!_created)
            {
                _created = true;

                Database.EnsureCreated();
            }
        }

        public DbSet<Brochure> Brochures { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString ?? defaultConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //--> In-Place model configuration or...
            builder.Entity<Brochure>().HasKey(x => x.Id);
            builder.Entity<Brochure>().Property(x => x.Id).HasColumnName(nameof(Brochure) + nameof(Brochure.Id));

            //--> Via Delegate model configuration or...
            builder.Entity<Brochure>(BrochureConfiguration);

            //--> Via Static Method model configuration or...
            ConfigureBrochure(builder);

            //--> Via Custom Extension Method model configuration or finally...
            builder.AddConfiguration<Brochure, BrochureConfiguration>();
            builder.AddConfiguration<BrochureConfiguration>();
            builder.AddConfiguration(new BrochureConfiguration());

            //--> Via Custom Fluent Interface model configuration.
            builder.Configurations()
                   .Add<BrochureConfiguration>()
                   .Add(new BrochureConfiguration())
                   .End();
        }


        private void BrochureConfiguration(EntityTypeBuilder<Brochure> etb)
        {
            etb.HasKey(x => x.Id);
            etb.Property(x => x.Id).HasColumnName(nameof(Brochure) + nameof(Brochure.Id));
        }

        private static void ConfigureBrochure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brochure>(b =>
            {
                b.HasKey(c => c.Id);
                b.Property(e => e.Id).HasColumnName(nameof(Brochure) + nameof(Brochure.Id));
            });
        }


        private void AuditEntities()
        {
            var addedEntities = ChangeTracker.Entries<IAuditableEntity>()
                                        .Where(p => p.State == EntityState.Added)
                                        .Select(p => p.Entity);

            var modifiedEntities = ChangeTracker.Entries<IAuditableEntity>()
                                                       .Where(p => p.State == EntityState.Modified)
                                                       .Select(p => p.Entity);

            var now = DateTime.UtcNow;

            foreach (var added in addedEntities)
            {
                added.CreateDate = now;
            }

            foreach (var modified in modifiedEntities)
            {
                modified.EditDate = now;
            }
        }

        public override int SaveChanges()
        {
            AuditEntities();

            return base.SaveChanges();
        }
    }
}
