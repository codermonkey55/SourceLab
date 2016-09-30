using Insight.Database;

namespace ODAC_Sample_InsightDatabase
{
    public class InsightDbProvider
    {
        public static void RegisterSqlProvider()
        {
            SqlInsightDbProvider.RegisterProvider();
        }
    }
}
