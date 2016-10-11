using Autofac;
using CodeLabs.Web.WebForms.IoC_Integration.IoC_Core.Autofac;
using Owin;
using System.Web.Http;

namespace CodeLabs.Web.WebForms.IoC_Integration.IoC_Configs
{
    public class AutofacConfig
    {
        private static IContainer ConfigureMvcContainer()
        {
            return AutofacMvcBuilder.BuildRegistrations();
        }

        private static IContainer ConfigureWebApiContainer()
        {
            return AutofacWebApiBuilder.BuildRegistrations();
        }

        public static void Bootstrap()
        {
            var mvcContainer = ConfigureMvcContainer();

            var webapiContainer = ConfigureWebApiContainer();
        }

        public static void Bootstrap(IAppBuilder app)
        {
            var mvcContainer = ConfigureMvcContainer();

            app.UseAutofacMiddleware(mvcContainer);

            app.UseAutofacWebApi(GlobalConfiguration.Configuration);

            app.UseAutofacMvc();
        }
    }
}
