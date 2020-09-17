using System;

namespace ModelSaber.Common
{
    public class Vote
    {
        /// <summary>
        /// The ID of the model statistic
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The user who voted the model.
        /// </summary>
        public User Voter { get; set; }

        /// <summary>
        /// The ID of the object that's being commented on.
        /// </summary>

        public Guid Source { get; set; }

        /// <summary>
        /// Is this vote positive?
        /// </summary>
        public bool IsUpvote { get; set; }
    }
}