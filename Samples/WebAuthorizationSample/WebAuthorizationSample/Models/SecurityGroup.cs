namespace WindowsAuthenticationSample.Models
{
    public struct SecurityGroup
    {
        public string Name { get; }

        public string ProfileIdentifier { get; }

        public SecurityGroup(string name, string profileIdentifier)
        {
            Name = name;
            ProfileIdentifier = profileIdentifier;
        }
    }
}