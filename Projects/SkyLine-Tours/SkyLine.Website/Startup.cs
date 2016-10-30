using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SkyLine.Website.Startup))]
namespace SkyLine.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
