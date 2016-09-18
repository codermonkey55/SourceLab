using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace WindowsAuthenticationSample.Models
{
    public interface IProfilePrincipal : IPrincipal
    {
        IReadOnlyCollection<string> Profiles { get; }
        IReadOnlyCollection<SecurityGroup> SecurityGroups { get; }

        IReadOnlyCollection<AuthorizedAction> AuthorizedActions { get; }

        bool IsAuthorized();

        bool HasAccess(string[] profileActions);

        bool HasAccess(string profile);
    }

    public class User : WindowsPrincipal, IProfilePrincipal
    {
        public IReadOnlyCollection<string> Profiles { get; set; }

        public IReadOnlyCollection<SecurityGroup> SecurityGroups { get; set; }

        public IReadOnlyCollection<AuthorizedAction> AuthorizedActions { get; set; }

        public User(WindowsIdentity ntIdentity) : base(ntIdentity) { }

        public bool IsAuthorized()
        {
            return SecurityGroups != null && SecurityGroups.Count > 0;
        }

        public bool HasAccess(string[] profileActions)
        {
            return AuthorizedActions.Select(aa => aa.Name).Intersect(profileActions).Any();
        }

        public bool HasAccess(string profile)
        {
            return AuthorizedActions.Any(aa => aa.ProfileIdentifier == profile);
        }
    }
}