using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WindowsAuthenticationSample.Constants;
using WindowsAuthenticationSample.Models;

namespace WindowsAuthenticationSample
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            DependencyResolver.Current.GetService<IUserManager>().InitUserProfilePrincipal();

            //var userProfiles = userManager.GetUserProfiles();
            //var securityGroups = userManager.GetUserSecurityGroups();
            //var authorizedActions = userManager.GetUserAuthorizedActions();

            //if (User.Identity.IsAuthenticated)
            //{
            //    var userIdentity = User.Identity as WindowsIdentity;
            //    var user = new User(userIdentity)
            //    {
            //        Profiles = userProfiles,
            //        SecurityGroups = securityGroups,
            //        AuthorizedActions = authorizedActions
            //    };
            //    HttpContext.Current.User = user;
            //    Thread.CurrentPrincipal = user;
            //}
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var profilePrincipal = User as IProfilePrincipal;
            var hasAccess = profilePrincipal?.HasAccess(Profiles.Amity) ?? false;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            var profilePrincipal = User as IProfilePrincipal;
            var hasAccess = profilePrincipal?.HasAccess(Profiles.Amity) ?? false;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}