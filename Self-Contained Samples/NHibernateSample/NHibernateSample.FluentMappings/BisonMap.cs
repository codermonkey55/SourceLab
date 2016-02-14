using FluentNHibernate.Mapping;
using NHibernateSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.FluentMappings
{
    public class BisonMap : ClassMap<Bison>
    {
        public BisonMap()
        {
            this.Id(x => x.Id, "BisonId").Not.Nullable().GeneratedBy.Assigned();
            this.Map(x => x.FurThickness);

            this.Map(x => x.HasTeeth);
            this.Map(x => x.HasFur);
            this.Map(x => x.HasHorns);
            this.Map(x => x.Name);
            this.Map(x => x.Description);

            this.Map(x => x.CreateDate);
            this.Map(x => x.EditDate);
        }
    }
}
