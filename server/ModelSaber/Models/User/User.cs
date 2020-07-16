using ModelSaber.Models.Discord;

namespace ModelSaber.Models.User
{
    public class User
    {
        public string ID { get; set; }
        public DiscordUser Profile { get; set; }
        public string[] Permissions { get; set; } = new string[] { "*.upload" };
        public string[] ExternalProfiles { get; set; } = new string[0]; 
    }
}