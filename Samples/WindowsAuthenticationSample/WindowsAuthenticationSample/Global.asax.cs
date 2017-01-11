using System;
using System.Security.Principal;
using System.Web;
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
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            (Context.User as IProfilePrincipal)?.OverrideSystemPrincipals();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Method intentionally left empty.
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Method intentionally left empty.
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Method intentionally left empty.
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
            // Method intentionally left empty.
        }

        public bool IsExpiredSession()
        {
            if (Context.Session == null)
                return false;

            bool isLogOutPathRequest = false;

            if (Request.UrlReferrer != null)
            {
                isLogOutPathRequest = Request.UrlReferrer.AbsolutePath.Contains("LogOutUrl");
            }

            if (Session.IsNewSession && !isLogOutPathRequest)
            {
                // - Use when triggering browser to delete ASP.NET Session Cookie.
                string cookiesHeader = Request.Headers["Cookie"];
                bool isExpiredSession1 = cookiesHeader != null && cookiesHeader.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) >= 0;


                //-> Use when ressetting ASP.NET Session Cookie.
                HttpCookie sessionCookie = Request.Cookies["ASP.NET_SessionId"];
                var isExpiredSession2 = sessionCookie?.Value != null;

                //-> KB Reference...sessionCookie.HttpOnly is set to true when a new ASP.NET session cookie has been created, but only after the old SP.NET session cookie as has expired/been deleted.
                //-> bool isNewSession = sessionCookie?.HttpOnly ?? false;"

                return isExpiredSession1 || isExpiredSession2;
            }

            return false;
        }
    }
}