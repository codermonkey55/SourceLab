using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernateSample.FluentMappings;

namespace NHibernateSample
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory CreateSessionFactory()
        {
            ISessionFactory sessionFactory =
            Fluently.Configure()
                    .Database(() =>
                    {
                        return MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("NHibernateSampleDb"))
                                                           .Dialect<MsSql2012Dialect>()
                                                           .Driver<SqlClientDriver>();
                    })
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<HabitatMap>()
                                        .Conventions.Add(ForeignKey.EndsWith("Id"))
                                        .Conventions.Add<TableNameConvention>();
                    })
                    .ExposeConfiguration(cfg =>
                    {
                        cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new AuditingEventListener() };
                        cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new AuditingEventListener() };
                    })
                    .BuildSessionFactory();

            return sessionFactory;
        }
    }

    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table("[dbo].[" + instance.EntityType.Name + "]");
        }
    }
}
