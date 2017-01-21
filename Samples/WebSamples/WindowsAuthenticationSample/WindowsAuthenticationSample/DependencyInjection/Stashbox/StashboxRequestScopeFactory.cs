using System.Web;
using Stashbox.Infrastructure;

namespace WindowsAuthenticationSample.DependencyInjection.Stashbox
{
    public class StashboxRequestScopeFactory
    {
        public IDependencyResolver GenerateRequestScope()
        {
            HttpContext.Current.Items["$_RequestScope_$"] = default(IDependencyResolver);

            return null;
        }
    }
}