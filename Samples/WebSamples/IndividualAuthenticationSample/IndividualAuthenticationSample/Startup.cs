using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IndividualAuthenticationSample.Startup))]
namespace IndividualAuthenticationSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
