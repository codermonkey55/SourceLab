﻿using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSample.Database.Extensions
{
    internal static class ModelBuilderExtensions
    {
        internal static void AddConfiguration<TEntity, TEntityConfiguration>(this ModelBuilder mb)
            where TEntity : class
            where TEntityConfiguration : IEntityBuilder<TEntity> , new()
        {
            mb.Entity<TEntity>(new TEntityConfiguration().Build);
        }

        internal static void AddConfiguration<TEntityConfiguration>(this ModelBuilder mb) where TEntityConfiguration : IEntityBuilder, new()
        {
            var ec = new TEntityConfiguration();

            mb.Entity(ec.EntityType, ec.Build);
        }

        internal static void AddConfiguration(this ModelBuilder mb, IEntityBuilder ec)
        {
            if (ec == null) return;

            mb.Entity(ec.EntityType, ec.Build);
        }

        internal static IEntityConfigurations Configurations(this ModelBuilder mb)
        {
            return new EntityConfigurations(mb);
        }
    }
}
