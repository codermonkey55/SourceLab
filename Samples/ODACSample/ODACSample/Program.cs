using Insight.Database;
using ODAC_Sample_FluentDAO;
using ODAC_Sample_InsightDatabase;
using ODAC_Sample_NHibernate;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;
using ODAC_Sample_NHibernate.Entities.Hr;
using ODACSample.EF;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ODAC_Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //NHibernate_Example();

            //ADO_NET_Example();

            EntityFramework_Example();

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

        static void EntityFramework_Example()
        {
            var context = new OracleDbContext();

            var results = context.Products.Where(p => p.Category.CategoryName == "Beverages");

            foreach (var result in results)
            {
                Console.WriteLine("Product Name: {0} | Quantity Per Unit: {1}, | Unit Price: {2}", result.ProductName, result.QuantityPerUnit, result.UnitPrice);
            }
        }

        static void NHibernate_Example()
        {
            //var cs = "User Id=hr; Password=hr;data source=HR_TNS;";
            //var cs = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=db_user;Password=dbuser123;";
            var cs = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=www.machinejar.com)(PORT=1522)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devorc02)));User Id=hr;Password=hr;";

            var nhconfig = NHibernateConfig.Instance(cs);

            //var nhconfig = NHibernateConfig.Instance();

            var session = nhconfig.SessionFactory.OpenSession();

            //var query = session.CreateSQLQuery("CALL HR_DEPTPERCOUNTRY (:RS_CURSOR, :CNTR_CD)");

            var query = session.GetNamedQuery("HR_DEPTPERCOUNTRY");

            query.SetString("CNTR_CD", "UK");

            //query.AddEntity(typeof(HumanResourcesInfo));

            //query.SetResultTransformer(Transformers.AliasToBean<HumanResourcesInfo>());

            var results = query.List<HumanResourcesInfo>();

            foreach (var result in results)
            {
                Console.WriteLine("Dept Name: {0} | Address: {1}, | Country: {2}", result.Department, result.Address, result.Country);
            }
        }

        static void ADO_NET_Example()
        {
            var cs = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=www.machinejar.com)(PORT=1522)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=devorc02)));User Id=hr;Password=hr;";

            OracleConnection con = new OracleConnection();
            con.ConnectionString = cs;
            con.Open();

            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "HR_DEPTPERCOUNTRY";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            OracleParameter orcOutParam = new OracleParameter("RS_CURSOR", OracleDbType.RefCursor);
            orcOutParam.Size = 50;
            orcOutParam.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(orcOutParam);

            OracleParameter orcParam = new OracleParameter("CNTR_CD", OracleDbType.Varchar2);
            orcParam.Size = 50;
            orcParam.Value = "UK";
            orcParam.Direction = System.Data.ParameterDirection.Input;
            cmd.Parameters.Add(orcParam);

            cmd.ExecuteNonQuery();

            OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["RS_CURSOR"].Value).GetDataReader();

            while (reader.Read())
            {
                Console.WriteLine("Dept Name: {0} | Address: {1}, | Country: {2}", reader.GetString(0), reader.GetString(1), reader.GetString(2));
            }
        }
    }
}
