using FluentNHibernate.Mapping;
using ODAC_Sample_NHibernate.Entities.Hr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODAC_Sample_NHibernate.Mappings
{
    public sealed class HrInfoMap : ClassMap<HumanResourcesInfo>
    {
        public HrInfoMap()
        {
            Id(ent => ent.Id);
            Map(ent => ent.Department_Name);
            Map(ent => ent.Street_Address);
            Map(ent => ent.Country_Name);
        }
    }
}
