using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using QuickSnacks.Data.NHibernate.Entities.Base;

namespace QuickSnacks.Data.NHibernate.FluentMappings.BaseMaps
{
    public class AuditMap<TEntity> : CommonMap<TEntity> where TEntity : AuditableEntity
    {
        public AuditMap()
        {
            this.Map(x => x.CreateDate);

            this.Map(x => x.EditDate);

            this.Component(x => x.AuditInfo);
        }
    }
}
