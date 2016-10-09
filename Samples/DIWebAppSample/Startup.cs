using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DIWebAppSample.Startup))]
namespace DIWebAppSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
