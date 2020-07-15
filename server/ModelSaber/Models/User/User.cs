using ModelSaber.Models.Discord;

namespace ModelSaber.Models.User
{
    public class User
    {
        public uint ID { get; set; }
        public DiscordUser Profile { get; set; }
        public string[] Permissions { get; set; }
        public string[] ExternalProfiles { get; set; }
    }
}