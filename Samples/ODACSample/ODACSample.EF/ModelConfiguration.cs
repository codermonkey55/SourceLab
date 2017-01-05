using Oracle.ManagedDataAccess.EntityFramework;
using System.Data.Entity;

namespace ODACSample.EF
{
    public class ModelConfiguration : DbConfiguration
    {
        public ModelConfiguration()
        {
            SetProviderServices("Oracle.ManagedDataAccess.Client", EFOracleProviderServices.Instance);
        }
    }
}
