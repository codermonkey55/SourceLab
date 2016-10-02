using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using ODAC_Sample_NHibernate.Mappings;

namespace ODAC_Sample_NHibernate
{
    public class NHibernateConfig
    {
        private readonly string _connectionString = string.Empty;
        private ISessionFactory _sessionFactory = default(ISessionFactory);

        public ISessionFactory SessionFactory
        {
            get
            {
                return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
            }
        }

        protected NHibernateConfig(string connectionString)
        {
            _connectionString = connectionString;
            _sessionFactory = CreateSessionFactory();
        }

        public ISession Session
        {
            get { return SessionFactory.OpenSession(); }
        }

        public static NHibernateConfig Instance(string connectionString)
        {
            return new NHibernateConfig(connectionString);
        }

        private ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure();

            try
            {
                config.Database(
                    OracleDataClientConfiguration.Oracle10
                    .ConnectionString(_connectionString)
                    //.Driver<NHibernate.Driver.OracleManagedDataClientDriver>()
                    .Dialect<NHibernate.Dialect.Oracle10gDialect>()
                    .Driver<NHibernate.Driver.OracleDataClientDriver>());

                //config.DataBaseIntegration(db => {
                //    db.ConnectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=your_host)(PORT=your_port)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=your_service)));User Id=your_user;Password=your_password;";
                //    db.Driver<NHibernate.Driver.OracleManagedDriver>();
                //    db.Dialect<NHibernate.Dialect.Oracle10gDialect>();
                //    db.SchemaAction = SchemaAutoAction.Validate;
                //});

                config.Mappings(m => m.FluentMappings.AddFromAssemblyOf<HrInfoMap>());
                config.Mappings(m => m.HbmMappings.AddFromAssemblyOf<HrInfoMap>());
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return config.BuildSessionFactory();
        }
    }
}
