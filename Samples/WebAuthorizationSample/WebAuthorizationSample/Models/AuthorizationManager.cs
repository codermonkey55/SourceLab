using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace WindowsAuthenticationSample.Models
{
    public interface IAuthorizationManager
    {
        IPrincipal GetAuthenticatedUser();

        bool IsAuthorized(IEnumerable<SecurityGroup> securityGroups);

        bool IsValidSecurityGroup(string securityGroup);
    }

    public class AuthorizationManager : IAuthorizationManager
    {
        public IPrincipal GetAuthenticatedUser()
        {
            //AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            return Thread.CurrentPrincipal as WindowsPrincipal;
        }

        public bool IsAuthorized(IEnumerable<SecurityGroup> securityGroups)
        {
            var authenticatedUser = GetAuthenticatedUser();

            var secGroups = securityGroups as SecurityGroup[] ?? securityGroups?.ToArray();

            if (secGroups == null || secGroups.Any())
                return false;

            foreach (var secGroup in secGroups)
                if (authenticatedUser.IsInRole(secGroup.Name))
                    return true;

            return false;
        }

        public bool IsValidSecurityGroup(string securityGroup)
        {
            var authenticatedUser = GetAuthenticatedUser();

            if (authenticatedUser.IsInRole(securityGroup))
                return true;

            return false;
        }
    }
}