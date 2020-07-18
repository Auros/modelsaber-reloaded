using ModelSaber.Models.Discord;
using System.Collections.Generic;

namespace ModelSaber.Models.User
{
    public class User
    {
        public string ID { get; set; }
        public DiscordUser Profile { get; set; }
        public ModelSaberRole Role { get; set; }
        public List<string> ExternalProfiles { get; set; } = new List<string>();
    }
}