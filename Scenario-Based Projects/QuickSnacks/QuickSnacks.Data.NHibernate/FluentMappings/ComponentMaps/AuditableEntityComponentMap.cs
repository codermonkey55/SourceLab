using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using QuickSnacks.Data.NHibernate.Entities;
using QuickSnacks.Data.NHibernate.Entities.Components;

namespace QuickSnacks.Data.NHibernate.FluentMappings.ComponentMaps
{
    public class AuditableEntityComponentMap : ComponentMap<AuditInfo>
    {
        public AuditableEntityComponentMap()
        {
            this.Map(x => x.CreateDate);

            this.Map(x => x.EditDate);
        }
    }
}
