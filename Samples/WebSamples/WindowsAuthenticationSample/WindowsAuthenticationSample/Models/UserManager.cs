using Stashbox.Attributes;
using System;
using System.Collections.Generic;

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
        private IReadOnlyCollection<string> _userProfiles;
        private readonly TAuthorizationManager _authorizationManager;
        private readonly TProfileConfiguration _profileConfiguration;

        public UserManager() { }

        [InjectionConstructor]
        public UserManager(TAuthorizationManager authorizationManager, TProfileConfiguration profileConfiguration)
        {
            _authorizationManager = authorizationManager;
            _profileConfiguration = profileConfiguration;

            InitUserProfiles();
        }

        public void InitUserProfilePrincipal()
        {
            GetUserProfilePrincipal().OverrideSystemPrincipals();
        }

        public IProfilePrincipal GetUserProfilePrincipal()
        {
            Func<IProfilePrincipal> provider;

            return (provider = () =>
            {
                IProfilePrincipal user = new User();
                var userProfiles = GetUserProfiles();
                var securityGroups = GetUserSecurityGroups();
                var authorizedActions = GetUserAuthorizedActions();
                var ntUser = _authorizationManager.GetAuthenticatedUser();
                if (ntUser.Identity.IsAuthenticated)
                    user = new User
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

                foreach (var profile in _userProfiles)
                    if (_profileConfiguration.SecurityGroupsByProfile.ContainsKey(profile))
                        foreach (var secGroup in _profileConfiguration.SecurityGroupsByProfile[profile])
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

                foreach (var profile in _userProfiles)
                    if (_profileConfiguration.AuthorizedActionsByProfile.ContainsKey(profile))
                        foreach (var authAction in _profileConfiguration.AuthorizedActionsByProfile[profile])
                            userAuthorizedActions.Add(new AuthorizedAction(authAction, profile));

                return userAuthorizedActions;
            })();
        }

        public IReadOnlyCollection<string> GetUserProfiles()
        {
            Func<IReadOnlyCollection<string>> provider;

            return _userProfiles ?? (provider = () =>
            {
                var userProfiles = new List<string>();

                foreach (var profile in _profileConfiguration.GetProfiles())
                    if (_profileConfiguration.SecurityGroupsByProfile.ContainsKey(profile))
                        foreach (var secGroup in _profileConfiguration.SecurityGroupsByProfile[profile])
                        {
                            if (!_authorizationManager.IsValidSecurityGroup(secGroup)) continue;
                            userProfiles.Add(profile);
                            break;
                        }


                return userProfiles;
            })();
        }

        private void InitUserProfiles()
        {
            _userProfiles = GetUserProfiles();
        }
    }
}