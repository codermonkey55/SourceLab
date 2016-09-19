﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using QuickSnacks.Data.NHibernate.AutoMappings;
using QuickSnacks.Data.NHibernate.Conventions;
using QuickSnacks.Data.NHibernate.Entities;
using QuickSnacks.Data.NHibernate.Entities.Base;
using QuickSnacks.Data.NHibernate.FluentMappings.ClassMaps;
using QuickSnacks.Data.NHibernate.Interceptors;
using QuickSnacks.Data.NHibernate.Listeners;
using Environment = NHibernate.Cfg.Environment;

namespace QuickSnacks.Data.NHibernate.Database
{
    public abstract class SessionFactoryBuilder
    {
        public static ISessionFactory CreateSessionFactory()
        {
            try
            {
                var config = new Configuration();
                Fluently.Configure(config)
                        .Database(() =>
                        {
                            var oracleDbConfig = OracleClientConfiguration.Oracle10.ConnectionString(c =>
                            {
                                c.Server("db_server");
                                c.Instance("db_name");
                                c.OtherOptions("other orc db cfg options");
                                c.Username("username");
                                c.Password("password");
                                c.Port(1521);
                                c.Pooling(true);
                            })
                            .Dialect<Oracle10gDialect>()
                            .Driver<OracleManagedDataClientDriver>()
                            .IsolationLevel(IsolationLevel.ReadCommitted)
                            .DefaultSchema("dbo")
                            .AdoNetBatchSize(100)
                            .FormatSql()
                            .UseReflectionOptimizer()
                            .ShowSql();

                            var msSqlDbConfig = MsSqlConfiguration.MsSql2012.ConnectionString(c =>
                            {
                                c.FromConnectionStringWithKey("QuickSnacksDb");
                                c.FromAppSetting("QuickSnacksDb");
                                c.TrustedConnection();
                            })
                            .Dialect<MsSql2012Dialect>()
                            .Driver<SqlClientDriver>()
                            .IsolationLevel(IsolationLevel.Snapshot)
                            .DefaultSchema("dbo")
                            .AdoNetBatchSize(100)
                            .FormatSql()
                            .UseReflectionOptimizer()
                            .ShowSql();

                            return msSqlDbConfig ?? oracleDbConfig as IPersistenceConfigurer;
                        })
                        .Mappings(m =>
                        {
                            m.AutoMappings.Add(Automappings);

                            m.FluentMappings.AddFromAssemblyOf<BrochureMap>()
                                .Conventions.Setup(c =>
                                {
                                    c.Add(ForeignKey.EndsWith("Id"));
                                    c.Add<TableNameConvention>();
                                    c.Add<PropertyConvention>();
                                    c.Add<CollectionConvention>();
                                    c.Add<ComponentConvention>();
                                });
                        })
                        .Cache(csb =>
                        {
                            csb.UseQueryCache()
                               .UseSecondLevelCache()
                               .ProviderClass("NHibernate.Cache.HashtableCacheProvider, NHibernate");
                        })
                        .ExposeConfiguration(cfg =>
                        {
                            cfg.AddAssembly(Assembly.GetExecutingAssembly());
                            cfg.AddDirectory(new DirectoryInfo("Mapping files directory."));
                            cfg.AddMapping(new HbmMapping());
                            cfg.BuildMappings();
                            cfg.AddUrl("http://repository.cyrotek.org/nhibernate/configs");
                            cfg.DataBaseIntegration(di =>
                            {
                                di.ConnectionStringName = "default";
                                di.Driver<SqlClientDriver>();
                                di.Dialect<MsSql2008Dialect>();
                                di.IsolationLevel = IsolationLevel.RepeatableRead;
                                di.Timeout = 10;
                                di.BatchSize = 10;
                            });
                            cfg.AddProperties(new Dictionary<string, string>
                            {
                                { Environment.ConnectionString, string.Format("Data Source={0};Version=3;New=True;", "db_FileName") }
                            });
                            cfg.AddNamedQuery("", builder => { builder.Query = "CALL sp_name{p1, p2, p3}"; });
                            cfg.GenerateDropSchemaScript(new MsSql2012Dialect());
                            cfg.GenerateSchemaCreationScript(new MsSql2012Dialect());
                            cfg.GenerateSchemaUpdateScript(new MsSql2012Dialect(),
                                new DatabaseMetadata(null, Dialect.GetDialect()));

                            cfg.SetProperty("generate_statistics", "true");
                            cfg.SetProperty("timeout", "10");
                            cfg.EntityCache<Entity>(ccp => ccp.Strategy = EntityCacheUsage.ReadWrite);

                            //--> Clears all previous listeners for given listener type and adds the given listener object.
                            cfg.SetListener(ListenerType.FlushEntity, new AuditFieldsDirtyCheckingEventListener());

                            //--> Prefered api over directly accessing EventListeners property collection.
                            cfg.AppendListeners(ListenerType.PreInsert, new object[] { new AuditingEventListener() });
                            cfg.AppendListeners(ListenerType.PreUpdate, new object[] { new AuditingEventListener() });
                            cfg.AppendListeners(ListenerType.FlushEntity, new object[] { new AuditFieldsDirtyCheckingEventListener() });

                            //--> Favor above api over directly accessing EventListeners property collection.
                            cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new PrePersistAuditEventListener() };
                            cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new PrePersistAuditEventListener() };
                            cfg.EventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[] { new FlushEntityAuditEventListener() };

                            cfg.Interceptor = new EmptyInterceptor();
                            cfg.Interceptor.AfterTransactionBegin(default(ITransaction));
                            cfg.Interceptor.AfterTransactionCompletion(default(ITransaction));
                            cfg.SetInterceptor(new PreCommitInterceptor());
                            cfg.SetInterceptor(new TrackingInterceptor());

                            var schemaExport = new SchemaExport(cfg);
                            schemaExport.Create(true, true);

                        });

                config.Configure("Name of config file if different from defaut: hibernate.cfg.xml"); // Override fluent-configurations with the "hibernate.cfg.xml" file default style.

                ISessionFactory sessionFactory = config.BuildSessionFactory();

                return sessionFactory;
            }
            catch (FluentConfigurationException fce)
            {
                throw new Exception("NHibernate Configuration Error.", fce);
            }
        }

        private static AutoPersistenceModel Automappings()
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
