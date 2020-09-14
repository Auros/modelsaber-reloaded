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
        /// The discord profile of the user.
        /// </summary>
        public DiscordUser Profile { get; set; }


    }
}