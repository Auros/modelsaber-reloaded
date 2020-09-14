namespace ModelSaber.Common
{
    public class DiscordUser
    {
        private string _avatar;

        /// <summary>
        /// The ID of the discord profile.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The username of the discord profile.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The discriminator of the discord profile. This is the four numbers (#1234) at the end of a Discord name
        /// </summary>
        public string Discriminator { get; set; }

        /// <summary>
        /// The URL of the profile picture. This is automatically formatted if it's animated.
        /// </summary>
        public string Avatar
        {
            get => _avatar;
            set => _avatar = value.StartsWith("http") ? value : ("https://cdn.discordapp.com/avatars/" + Id + "/" + value + (value.Substring(0, 2) == "a_" ? ".gif" : ".png"));
        }
    }
}