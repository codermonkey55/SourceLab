using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using QuickSnacks.Data.NHibernate.Entities.Base;

namespace QuickSnacks.Data.NHibernate.FluentMappings.BaseMaps
{
    public class CommonMap<TEntity> : ClassMap<TEntity>
    {
        public CommonMap()
        {
            this.DynamicInsert();

            this.DynamicUpdate();

            this.Polymorphism.Explicit();
        }
    }
}
