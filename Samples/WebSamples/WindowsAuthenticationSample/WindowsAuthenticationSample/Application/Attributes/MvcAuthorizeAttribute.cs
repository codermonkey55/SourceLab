using System.Web;
using System.Web.Mvc;
using WindowsAuthenticationSample.Constants;
using WindowsAuthenticationSample.Models;

namespace WindowsAuthenticationSample.Application.Attributes
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var userManager = DependencyResolver.Current.GetService<IUserManager>();

            var profilePrincipal = filterContext.HttpContext.User as IProfilePrincipal;

            if (profilePrincipal?.HasAccess(Profiles.Amity) ?? false)
            {
                //-> Do nothing...
            }

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string privilegeLevels = string.Empty; //string.Join("", GetUserRights(httpContext.User.Identity.Name.ToString())); //-> Call another method to get rights of the user from DB.

            if (privilegeLevels.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}