using System.Text.Json.Serialization;

namespace ModelSaber.Models.Discord
{
    public class DiscordUser
    {
        private string _avatar;

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar
        {
            get => _avatar;
            set => _avatar = value.StartsWith("http") ? value : ("https://cdn.discordapp.com/avatars/" + ID + "/" + value + (value.Substring(0, 2) == "a_" ? ".gif" : ".png"));
        }
    }
}