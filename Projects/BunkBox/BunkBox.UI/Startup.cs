using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BunkBox.UI.Startup))]
namespace BunkBox.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
