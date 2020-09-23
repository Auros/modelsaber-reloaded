using System;

namespace ModelSaber.Common
{
    public class User
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The role of the user in the context of ModelSaber
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// The Markdown description (or bio) of the user.
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// The discord profile of the user.
        /// </summary>
        public DiscordUser Profile { get; set; }
    }
}