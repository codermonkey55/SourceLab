using FluentNHibernate.Mapping;
using ODAC_Sample_NHibernate.Entities.Hr;

namespace ODAC_Sample_NHibernate.Mappings
{
    public sealed class HrInfoMap : ClassMap<HumanResourcesInfo>
    {
        public HrInfoMap()
        {
            Id(ent => ent.Department);
            Map(ent => ent.Address);
            Map(ent => ent.Country);
        }
    }
}
