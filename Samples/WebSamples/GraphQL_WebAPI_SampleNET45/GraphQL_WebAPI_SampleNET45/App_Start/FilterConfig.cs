using System.Web;
using System.Web.Mvc;

namespace GraphQL_WebAPI_SampleNET45
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
