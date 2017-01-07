using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace WindowsAuthenticationSample.Models
{
    public interface IProfilePrincipal : IPrincipal
    {
        IReadOnlyCollection<string> Profiles { get; }

        IReadOnlyCollection<SecurityGroup> SecurityGroups { get; }

        IReadOnlyCollection<AuthorizedAction> AuthorizedActions { get; }

        void SetIdentity(IIdentity identity);

        void SetInternalPrincipal(IPrincipal principal);

        void OverrideSystemPrincipals();

        bool IsAuthorized();

        bool HasAccess(string profile);

        bool HasAccess(string[] profileActions);
    }

    public class User : IProfilePrincipal
    {
        private ClaimsPrincipal _internalPrincipal;

        private IReadOnlyCollection<string> _roles;

        public IIdentity Identity { get; private set; }

        public IReadOnlyCollection<string> Profiles { get; set; }

        private IReadOnlyCollection<SecurityGroup> _securityGroups;
        public IReadOnlyCollection<SecurityGroup> SecurityGroups
        {
            get { return _securityGroups; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(SecurityGroups));

                _securityGroups = value;
                _roles = _securityGroups.Select(sg => sg.Name).ToList();
            }
        }

        private IReadOnlyCollection<AuthorizedAction> _authorizedActions;
        public IReadOnlyCollection<AuthorizedAction> AuthorizedActions
        {
            get { return _authorizedActions; }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(AuthorizedActions));

                _authorizedActions = value;
            }
        }

        public User()
        {
            _roles = new List<string>();
        }

        public void SetIdentity(IIdentity identity)
        {
            Identity = identity;
        }

        public void SetInternalPrincipal(IPrincipal principal)
        {
            _internalPrincipal = new ClaimsPrincipal(principal);

            SetIdentity(_internalPrincipal.Identity);
        }

        public void OverrideSystemPrincipals()
        {
            SetInternalPrincipal(Thread.CurrentPrincipal);

            Thread.CurrentPrincipal = this;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = this;
                HttpContext.Current.Session[nameof(IProfilePrincipal)] = this;
            }
        }

        public bool IsAuthorized()
        {
            return SecurityGroups.Count > 0;
        }

        public bool HasAccess(string profile)
        {
            return AuthorizedActions.Any(aa => aa.ProfileIdentifier == profile);
        }

        public bool HasAccess(string[] profileActions)
        {
            return AuthorizedActions.Select(aa => aa.Name).Intersect(profileActions).Any();
        }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role) || (_internalPrincipal?.IsInRole(role) ?? false);
        }
    }
}