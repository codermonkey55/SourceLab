using static WindowsAuthenticationSample.Constants.Profiles;

namespace WindowsAuthenticationSample.Constants
{
    public class ProfileGroups
    {
        public const string SectionAppendKey = ".Security.Groups";

        public const string MurphSecurityGroups = Murph + SectionAppendKey;
        public const string HuntsmanSecurityGroups = Huntsman + SectionAppendKey;
        public const string QuadrantSecurityGroups = Quadrant + SectionAppendKey;

        public const string AbnegationSecurityGroups = Abnegation + SectionAppendKey;
        public const string EruditeSecurityGroups = Erudite + SectionAppendKey;
        public const string DauntlessSecurityGroups = Dauntless + SectionAppendKey;
        public const string AmitySecurityGroups = Amity + SectionAppendKey;
        public const string CandorSecurityGroups = Candor + SectionAppendKey;
    }
}