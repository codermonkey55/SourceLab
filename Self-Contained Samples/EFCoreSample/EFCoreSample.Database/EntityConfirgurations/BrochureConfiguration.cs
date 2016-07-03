using System;
using EFCoreSample.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreSample.Database
{
    public class BrochureConfiguration : IEntityBuilder<Brochure>
    {
        public Type EntityType { get; private set; }

        public BrochureConfiguration()
        {
            this.EntityType = typeof(Brochure);
        }

        public void Build(EntityTypeBuilder<Brochure> etb)
        {
            etb.HasKey(x => x.Id);

            etb.Property(x => x.Id).HasColumnName(nameof(Brochure) + nameof(Brochure.Id));
        }

        public void Build(EntityTypeBuilder etb)
        {
            var entity = new Brochure();

            etb.HasKey(nameof(Brochure.Id));

            etb.Property(entity.Id.GetType(), nameof(Brochure.Id)).HasColumnName(nameof(Brochure) + nameof(Brochure.Id));
        }
    }
}
