using System;
using System.Web;

namespace WindowsAuthenticationSample.DependencyInjection.Stashbox
{
    public class StashboxRequestScopeModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        static void OnEndRequest(object sender, EventArgs e)
        {

        }
    }
}