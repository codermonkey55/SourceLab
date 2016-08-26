using System;
using EFCoreSample.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCoreSample.Database.Extensions
{
    public static class ContextExtensions
    {
        public static void SaveOrUpdate<TKey>(this DbContext context, Entity<TKey> entity)
            where TKey : struct
        {
            EntityEntry entityEntry = context.Entry(entity);

            if (entityEntry == null) throw new InvalidOperationException("EntityEntry was null.");

            switch (entityEntry.State)
            {
                case EntityState.Added:
                    context.Add(entity);
                    break;

                case EntityState.Modified:
                    context.Update(entity);
                    break;

                case EntityState.Detached:
                    context.Attach(entity);
                    break;
            }
        }
    }
}
