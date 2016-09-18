namespace WindowsAuthenticationSample.Models
{
    public struct AuthorizedAction
    {
        public string Name { get; }

        public string ProfileIdentifier { get; }

        public AuthorizedAction(string name, string profileIdentifier)
        {
            Name = name;
            ProfileIdentifier = profileIdentifier;
        }
    }
}