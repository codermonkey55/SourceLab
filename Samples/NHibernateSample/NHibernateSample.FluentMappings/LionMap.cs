using FluentNHibernate.Mapping;
using NHibernateSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.FluentMappings
{
    public class LionMap : ClassMap<Lion>
    {
        public LionMap()
        {
            this.Id(x => x.Id, "LionId");
            this.Map(x => x.TeethCount);

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
