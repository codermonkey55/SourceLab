using Autofac;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Autofac;
using Owin;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class AutofacConfig
    {
        private static IContainer ConfigureContainer()
        {
            return AutofacWebBuilder.BuildRegistrations();
        }

        public static void Bootstrap()
        {
            var container = ConfigureContainer();
        }

        public static void Bootstrap(IAppBuilder app)
        {
            var container = ConfigureContainer();

            app.UseAutofacMiddleware(container);

            app.UseAutofacMvc();
        }
    }
}
