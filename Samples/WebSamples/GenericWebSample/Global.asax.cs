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
			var exceptionMessage = User.Identity.Name;
			Logger.WriteError(exceptionMessage);

			if (Response.HeadersWritten) return;

			Server.ClearError();
			Response.RedirectToRoute("RouteNameForUnhandledExceptions");
		}

		protected void Application_BeginRequest()
		{
			if (Request.Url.AbsolutePath.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
			{
				Response.RedirectToRoute("RouteMameForMvcDefaultPage");
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
				Response.RedirectToRoute("RouteMameForMvcDefaultPage");
			}
			else
			{
				try
				{
					var loginFailureMessage = new StringBuilder();
					loginFailureMessage.AppendLine("Message: Requesting user is not authorized to use this application.");
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
						Logger.WriteError($"Message: Error encoutered while logging failed authentication attempt by current-user. \nError: {ex.Message}");
					}
				}

				//-> "/" Represents the application root.
				if (Request.Url.AbsolutePath.Equals("/"))
				{
					var unAuthorizedRoute = RouteTable.Routes["RouteMameForApplicationUnAuthorizedPage"] as Route;
					if (unAuthorizedRoute == null)
						throw new ArgumentNullException(nameof(unAuthorizedRoute), $"Named route: {"RouteMameForApplicationUnAuthorizedPage"}, was not found in the Mvc RouteDictionary with the given name.");

					Response.Clear();
					Response.Redirect(unAuthorizedRoute.Url);
					Server.ClearError();
				}
				else
				{
					Response.RedirectToRoute("RouteMameForApplicationUnAuthorizedPage");
				}
			}
		}

		private bool IsSessionExpired()
		{
			return false;
		}
	}
}