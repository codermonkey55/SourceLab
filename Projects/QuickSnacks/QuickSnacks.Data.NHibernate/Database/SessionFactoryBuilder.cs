using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using QuickSnacks.Data.NHibernate.AutoMappings;
using QuickSnacks.Data.NHibernate.Conventions;
using QuickSnacks.Data.NHibernate.Entities;
using QuickSnacks.Data.NHibernate.Entities.Base;
using QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps;
using QuickSnacks.Data.NHibernate.Utilities;

namespace QuickSnacks.Data.NHibernate.Database
{
    public abstract class SessionFactoryBuilder
    {
        public static ISessionFactory CreateSessionFactory()
        {
            try
            {
                ISessionFactory sessionFactory =
                Fluently.Configure()
                        .Database(() =>
                        {
                            //return OracleClientConfiguration.Oracle10.ConnectionString(c => c.FromConnectionStringWithKey("QuickSnacksDb"))
                            //                                   .Dialect<Oracle10gDialect>()
                            //                                   .Driver<OracleManagedDataClientDriver>();

                            return MsSqlConfiguration.MsSql2012.ConnectionString(c => c.FromConnectionStringWithKey("QuickSnacksDb"))
                                                               .Dialect<MsSql2012Dialect>()
                                                               .Driver<SqlClientDriver>();
                        })
                        .Mappings(m =>
                        {
                            m.AutoMappings.Add(CustomAutomappings);

                            m.FluentMappings.AddFromAssemblyOf<BrochureMap>()
                                            .Conventions.Setup(c =>
                                            {
                                                c.Add(ForeignKey.EndsWith("Id"));
                                                c.Add<TableNameConvention>();
                                                c.Add<PropertyConvention>();
                                                c.Add<CollectionConvention>();
                                                c.Add<ComponentConvention>();
                                            })
                                            ;
                        })
                        .ExposeConfiguration(cfg =>
                        {
                            //--> Clears all previous listeners for given listener type and adds the given listener object.
                            cfg.SetListener(ListenerType.FlushEntity, new AuditFieldsDirtyCheckingEventListener());

                            //--> Prefered api over directly accessing EventListeners property collection.
                            cfg.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new AuditingEventListener() });
                            cfg.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { new AuditingEventListener() });
                            cfg.AppendListeners(ListenerType.FlushEntity, new IFlushEntityEventListener[] { new AuditFieldsDirtyCheckingEventListener() });

                            //--> Favor above api over directly accessing EventListeners property collection.
                            //cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new PrePersistAuditEventListener() };
                            //cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new PrePersistAuditEventListener() };
                            //cfg.EventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[] { new FlushEntityAuditEventListener() };
                        })
                        .BuildSessionFactory();

                return sessionFactory;
            }
            catch (FluentConfigurationException e)
            {
                throw e;
            }
        }

        private static AutoPersistenceModel CustomAutomappings()
        {
            return AutoMap.AssemblyOf<Brochure>(new AutoMappingConfiguration())
                          .Conventions.Setup(c =>
                           {
                               c.Add(ForeignKey.EndsWith("Id"));
                               c.Add<TableNameConvention>();
                               c.Add<PropertyConvention>();
                               c.Add<CollectionConvention>();
                               c.Add<ComponentConvention>();
                           })
                          .IgnoreBase(typeof(Entity<>))
                          .IncludeBase<AuditableEntity>();
        }
    }
}
