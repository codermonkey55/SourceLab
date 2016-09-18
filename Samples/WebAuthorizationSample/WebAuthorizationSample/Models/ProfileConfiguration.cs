using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using static WindowsAuthenticationSample.Constants.ProfileActions;
using static WindowsAuthenticationSample.Constants.ProfileGroups;
using static WindowsAuthenticationSample.Constants.Profiles;

namespace WindowsAuthenticationSample.Models
{
    public interface IProfileConfiguration
    {
        IDictionary<string, IReadOnlyCollection<string>> SecurityGroupsByProfile { get; }

        IDictionary<string, IReadOnlyCollection<string>> AuthorizedActionsByProfile { get; }

        IReadOnlyCollection<string> GetProfiles();

        IReadOnlyCollection<string> GetProfileGroups();

        IReadOnlyCollection<string> GetProfileActions();
    }

    public class ProfileConfiguration : IProfileConfiguration
    {
        private static readonly string SecurityGroup = ".Security.Groups";
        private static readonly string AuthorizedAction = ".Actions";

        protected readonly IDictionary<string, string> ProfileGroups = new Dictionary<string, string>
        {
            [Murph] = Murph + SecurityGroup,
            [Huntsman] = HuntsmanSecurityGroups,
            [Quadrant] = QuadrantSecurityGroups,

            [Abnegation] = Abnegation + AuthorizedAction,
            [Erudite] = Erudite + AuthorizedAction,
            [Amity] = AmitySecurityGroups,
            [Dauntless] = DauntlessSecurityGroups,
            [Candor] = CandorSecurityGroups
        };

        protected readonly IDictionary<string, string> ProfileActions = new Dictionary<string, string>
        {
            [Abnegation] = AbnegationActions,
            [Erudite] = EruditeActions,
            [Amity] = AmityActions,
            [Dauntless] = DauntlessActions,
            [Candor] = CandorActions
        };

        public IDictionary<string, IReadOnlyCollection<string>> SecurityGroupsByProfile { get; }

        public IDictionary<string, IReadOnlyCollection<string>> AuthorizedActionsByProfile { get; }

        public ProfileConfiguration()
        {
            var securityGroupsByProfile = new Dictionary<string, IReadOnlyCollection<string>>();

            foreach (var profileGroup in ProfileGroups)
                securityGroupsByProfile.Add(profileGroup.Key, getCommaDelimitedList(ConfigurationManager.AppSettings[profileGroup.Value]));

            SecurityGroupsByProfile = securityGroupsByProfile;

            var authorizedActionsByProfile = new Dictionary<string, IReadOnlyCollection<string>>();

            foreach (var profileAction in ProfileActions)
                authorizedActionsByProfile.Add(profileAction.Key, getCommaDelimitedList(ConfigurationManager.AppSettings[profileAction.Value]));

            AuthorizedActionsByProfile = authorizedActionsByProfile;
        }

        protected IReadOnlyCollection<string> getCommaDelimitedList(string commaDelimitedString)
        {
            commaDelimitedString = commaDelimitedString?.Replace(" ", "");

            return Array.AsReadOnly(commaDelimitedString?.Split(',') ?? new string[0]);
        }

        public IReadOnlyCollection<string> GetProfiles()
        {
            return new List<string>
            {
                Murph,
                Huntsman,
                Quadrant,

                Abnegation,
                Erudite,
                Amity,
                Dauntless,
                Candor
            };
        }

        public IReadOnlyCollection<string> GetProfileGroups()
        {
            return ProfileGroups.Values.ToList();
        }

        public IReadOnlyCollection<string> GetProfileActions()
        {
            return ProfileActions.Values.ToList();
        }
    }
}