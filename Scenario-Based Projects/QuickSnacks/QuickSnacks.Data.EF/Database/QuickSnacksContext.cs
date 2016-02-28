using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSnacks.Data.EF.Entities;
using QuickSnacks.Data.EF.EntityConfigurations;

namespace QuickSnacks.Data.EF.Database
{
    public class QuickSnacksContext : DbContext
    {
        static QuickSnacksContext()
        {

        }

        public QuickSnacksContext() : base("name=QuickSnacksDb")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public QuickSnacksContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }


        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FingerItem> FingerItems { get; set; }
        public DbSet<Brochure> Brochures { get; set; }


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
                added.EditDate = now;
            }

            foreach (var modified in modifiedEntities)
            {
                modified.EditDate = now;
            }
        }

        private T SaveChangesWithErrorHandling<T>(Func<T> saveFunc)
        {
            try
            {
                return saveFunc.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                List<string> errorList = new List<string>();

                var outerException = e.InnerException; ;
                var innerException = outerException.InnerException;

                var outerExceptionMessage = outerException.Message;
                var innerExceptionMessage = innerException == null ? string.Empty : innerException.Message;

                errorList.Add("Outer Exception Message: " + outerExceptionMessage);
                errorList.Add("Inner Exception Message: " + innerExceptionMessage);

                foreach (var entityWithError in e.Entries.Select(x => x.Entity))
                {
                    errorList.Add("An Error Occurred updating Entity " + entityWithError.ToString());
                }

                var errors = string.Join("--|-- \n", errorList.ToArray());

                throw new DbUnexpectedValidationException(errors);
            }
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new BrochureConfiguration());
            modelBuilder.Configurations.Add(new FingerItemConfiguration());
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new MenuItemConfiguration());
            modelBuilder.Configurations.Add(new TokenConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());

 	        base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            AuditEntities();

            return base.SaveChanges();
        }

        public int SaveChanges(bool includeErrorHandling)
        {
            if (includeErrorHandling) return this.SaveChangesWithErrorHandling(SaveChanges);

            else return SaveChanges();
        }
    }
}
