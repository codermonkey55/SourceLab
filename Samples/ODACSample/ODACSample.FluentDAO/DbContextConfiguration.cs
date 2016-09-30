using FluentDAO;

namespace ODAC_Sample_FluentDAO
{
    public class DbContextConfiguration
    {
        public static IDbContext GetContext(string connectionStringName)
        {
            return new DbContext().ConnectionStringName(connectionStringName, new SqlServerProvider());
        }

        public static IDbContext GetContextUsing(string connectionString)
        {
            return new DbContext().ConnectionString(connectionString, new SqlServerProvider());
        }
    }
}
