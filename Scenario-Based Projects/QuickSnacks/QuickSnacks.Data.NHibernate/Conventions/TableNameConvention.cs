using System;
using System.Linq;
using System.Collections.Generic;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace QuickSnacks.Data.NHibernate.Conventions
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table("[dbo].[" + instance.EntityType.Name + "s]");
        }
    }
}