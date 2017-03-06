using GenericWebSample.Application.Constants;
using Microsoft.Owin.Logging;
using System;
using System.Diagnostics;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GenericWebSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public ILogger Logger { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            var exception = Server.GetLastError();
            if (exception == null) return;

            //-> Log all un-handled exceptions.
            var exceptionMessage = new StringBuilder();
            exceptionMessage.AppendLine($"Error-Message: {exception.Message}");
            exceptionMessage.AppendLine($"User: {User.Identity.Name}");
            Logger.WriteError(exceptionMessage.ToString());

            if (Response.HeadersWritten) return;

            Server.ClearError();
            Response.RedirectToRoute("RouteNameForUnhandledExceptions");
        }

        protected void Application_BeginRequest()
        {
            if (Request.Url.AbsolutePath.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                Response.RedirectToRoute(AppRoutes.RouteMameForMvcDefaultPage);
            }
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs args)
        {
            //-> Method intentionally left empty.
        }

        protected void Session_Start(object sender, EventArgs args)
        {
            if (IsSessionExpired()) return;

            //base.Session_Start();

            var authorizationManager = DependencyResolver.Current.GetService<IAuthorizationManager>();

            if (authorizationManager.IsValidApplicationUser())
            {
                //-> Irrelevant for application_start and if not app_start then originating request should have url pre-defined.
                Response.RedirectToRoute(AppRoutes.RouteMameForMvcDefaultPage);
            }
            else
            {
                try
                {
                    var loginFailureMessage = new StringBuilder();
                    loginFailureMessage.AppendLine($"Info-Message: The Requesting user is not authorized to use this application.");
                    loginFailureMessage.AppendLine($"User: {User.Identity.Name}");

                    Logger.WriteError(loginFailureMessage.ToString());
                }
                catch (Exception ex)
                {

                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                    else
                    {
                        var failedAuthenticationAttemptMessage = new StringBuilder();
                        failedAuthenticationAttemptMessage.AppendLine($"Info-Message: Error encountered while attempting to log a failed authentication attempt by the current-user.");
                        failedAuthenticationAttemptMessage.AppendLine($"Error-Message: {ex.Message}");
                        Logger.WriteError(failedAuthenticationAttemptMessage.ToString());
                    }
                }

                //-> "/" Represents the application root path.
                if (Request.Url.AbsolutePath.Equals(AppPaths.ApplicationRootPath))
                {
                    var unAuthorizedRoute = RouteTable.Routes[AppRoutes.RouteNameForApplicationUnAuthorizedPage] as Route;
                    if (unAuthorizedRoute == null)
                        throw new ArgumentNullException(nameof(unAuthorizedRoute), $"Named route: {AppRoutes.RouteNameForApplicationUnAuthorizedPage}, was not found in the Mvc Route Dictionary with the given name.");

                    Response.Clear();
                    Response.Redirect(unAuthorizedRoute.Url);
                    Server.ClearError();
                }
                else
                {
                    Response.RedirectToRoute(AppRoutes.RouteNameForApplicationUnAuthorizedPage);
                }
            }
        }

        private bool IsSessionExpired()
        {
            return false;
        }
    }
}