using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Stashbox.Attributes;

namespace WindowsAuthenticationSample.Models
{
    public interface IUserManager
    {
        void InitUserProfilePrincipal();
        IProfilePrincipal GetUserProfilePrincipal();
        IReadOnlyCollection<string> GetUserProfiles();
        IReadOnlyCollection<SecurityGroup> GetUserSecurityGroups();
        IReadOnlyCollection<AuthorizedAction> GetUserAuthorizedActions();
    }

    public class UserManager<TAuthorizationManager, TProfileConfiguration> : IUserManager
        where TAuthorizationManager : IAuthorizationManager
        where TProfileConfiguration : IProfileConfiguration
    {
        public IReadOnlyCollection<string> UserProfiles;

        public TAuthorizationManager AuthorizationManager;
        public TProfileConfiguration ProfileConfiguration;

        public UserManager()
        {

        }

        [InjectionConstructor]
        public UserManager(TAuthorizationManager authorizationManager, TProfileConfiguration profileConfiguration)
        {
            AuthorizationManager = authorizationManager;
            ProfileConfiguration = profileConfiguration;

            InitUserProfiles();
        }

        public void InitUserProfilePrincipal()
        {
            var user = GetUserProfilePrincipal();
            HttpContext.Current.User = user;
            Thread.CurrentPrincipal = user;
        }

        public IProfilePrincipal GetUserProfilePrincipal()
        {
            Func<IProfilePrincipal> provider;

            return (provider = () =>
            {
                IProfilePrincipal user = null;
                var userProfiles = GetUserProfiles();
                var securityGroups = GetUserSecurityGroups();
                var authorizedActions = GetUserAuthorizedActions();
                var adUser = AuthorizationManager.GetAuthenticatedUser();
                if (adUser.Identity.IsAuthenticated)
                    user = new User(adUser.Identity as WindowsIdentity)
                    {
                        Profiles = userProfiles,
                        SecurityGroups = securityGroups,
                        AuthorizedActions = authorizedActions
                    };

                return user;
            })();
        }

        public IReadOnlyCollection<SecurityGroup> GetUserSecurityGroups()
        {
            Func<IReadOnlyCollection<SecurityGroup>> provider;

            var set = new HashSet<string>();

            return (provider = () =>
            {
                var userSecurityGroups = new List<SecurityGroup>();

                foreach (var profile in UserProfiles)
                    if (ProfileConfiguration.SecurityGroupsByProfile.ContainsKey(profile))
                        foreach (var secGroup in ProfileConfiguration.SecurityGroupsByProfile[profile])
                            userSecurityGroups.Add(new SecurityGroup(secGroup, profile));

                return userSecurityGroups;
            })();
        }

        public IReadOnlyCollection<AuthorizedAction> GetUserAuthorizedActions()
        {
            Func<IReadOnlyCollection<AuthorizedAction>> provider;

            return (provider = () =>
            {
                var userAuthorizedActions = new List<AuthorizedAction>();

                foreach (var profile in UserProfiles)
                    if (ProfileConfiguration.AuthorizedActionsByProfile.ContainsKey(profile))
                        foreach (var authAction in ProfileConfiguration.AuthorizedActionsByProfile[profile])
                            userAuthorizedActions.Add(new AuthorizedAction(authAction, profile));

                return userAuthorizedActions;
            })();
        }

        public IReadOnlyCollection<string> GetUserProfiles()
        {
            Func<IReadOnlyCollection<string>> provider;

            return UserProfiles ?? (provider = () =>
            {
                var userProfiles = new List<string>();

                foreach (var profile in ProfileConfiguration.GetProfiles())
                    if (ProfileConfiguration.SecurityGroupsByProfile.ContainsKey(profile))
                        foreach (var secGroup in ProfileConfiguration.SecurityGroupsByProfile[profile])
                        {
                            if (!AuthorizationManager.IsValidSecurityGroup(secGroup)) continue;
                            userProfiles.Add(profile);
                            break;
                        }


                return userProfiles;
            })();
        }

        private void InitUserProfiles()
        {
            UserProfiles = GetUserProfiles();
        }
    }
}