﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Insight.Database;
using NHibernate.Transform;
using ODAC_Sample_FluentDAO;
using ODAC_Sample_InsightDatabase;
using ODAC_Sample_NHibernate;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;
using ODAC_Sample_NHibernate.Entities.Hr;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ODAC_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NHibernate_Example();

            //ADO_NET_Example();

            //FluentDAO_Example();

            //InsightDatabase_Example();

            Console.WriteLine();
            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
        }

        /// <summary>
        /// <see cref="https://github.com/jonwagner/Insight.Database"/>
        /// </summary>
        static void InsightDatabase_Example()
        {
            InsightDbProvider.RegisterSqlProvider();

            var conn = new SqlConnection(@"Data Source=www.machinejar.com\DEVSQL02;Initial Catalog=AdventureWorks2014;Persist Security Info=True;User ID=db_user;Password=***");

            var productAddresses = conn.QuerySql<dynamic>("select top 10 *  from Person.Address");

            Console.WriteLine(productAddresses.First().AddressLine1);

            Console.ReadLine();
        }

        /// <summary>
        /// <see cref="https://github.com/leadnt/FluentDAO"/>
        /// </summary>
        static void FluentDAO_Example()
        {
            var context = DbContextConfiguration.GetContextUsing(@"Data Source=www.machinejar.com\DEVSQL02;Initial Catalog=AdventureWorks2014;Persist Security Info=True;User ID=db_user;Password=***");

            List<dynamic> productAddresses = context.Sql("select top 10 *  from Person.Address").QueryMany<dynamic>();

            Console.WriteLine(productAddresses.First().AddressLine1);

            Console.ReadLine();
        }

        static void NHibernate_Example()
        {
            //var cs = "User Id=hr; Password=hr;data source=HR_TNS;";
            var cs = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=db_user;Password=dbuser123;";

            var nhconfig = NHibernateConfig.Instance(cs);

            //var nhconfig = NHibernateConfig.Instance();

            var session = nhconfig.SessionFactory.OpenSession();

            var query = session.CreateSQLQuery("exec HR_DEPTPERCOUNTRY :CNTR_CD");

            //var query = session.GetNamedQuery("exec HR_DEPTPERCOUNTRY :CNTR_CD");

            query.SetParameter("CNTR_CD", "US");

            query.AddEntity(typeof(HumanResourcesInfo));

            query.SetResultTransformer(Transformers.AliasToBean<HumanResourcesInfo>());

            var result = query.List<HumanResourcesInfo>();
        }

        static void ADO_NET_Example()
        {
            var cs = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=hr;Password=\"hr\";";

            OracleConnection con = new OracleConnection();
            con.ConnectionString = cs;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "HR_DEPTPERCOUNTRY";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter orcParam = new OracleParameter("CNTR_CD", OracleDbType.Varchar2);
            orcParam.Size = 50;
            orcParam.Value = "US";
            orcParam.Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add(orcParam);

            OracleParameter orcOutParam = new OracleParameter("RS_CURSOR", OracleDbType.RefCursor);
            orcOutParam.Size = 50;
            orcOutParam.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(orcOutParam);

            cmd.ExecuteNonQuery();

            OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["RS_CURSOR"].Value).GetDataReader();

            while (reader.Read())
            {
                Console.WriteLine("Dept Name: {0} | Address: {1}, | Country: {2}", reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }
    }
}
