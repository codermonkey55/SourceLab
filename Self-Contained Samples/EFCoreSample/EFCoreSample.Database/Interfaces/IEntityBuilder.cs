using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreSample.Database
{
    public interface IEntityBuilder
    {
        Type EntityType { get; }

        void Build(EntityTypeBuilder etb);
    }
}
