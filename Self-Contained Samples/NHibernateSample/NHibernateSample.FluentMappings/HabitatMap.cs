using FluentNHibernate.Mapping;
using NHibernateSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample.FluentMappings
{
    public class HabitatMap : ClassMap<Habitat>
    {
        public HabitatMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Area);

            this.Map(x => x.Name);
            this.Map(x => x.Description);
            this.Map(x => x.CreateDate);
            this.Map(x => x.EditDate);
        }
    }
}
