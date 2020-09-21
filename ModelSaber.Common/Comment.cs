using System;

namespace ModelSaber.Common
{
    public class Comment
    {
        /// <summary>
        /// The ID of the comment.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the object that's being commented on.
        /// </summary>
        public Guid Source { get; set; }

        /// <summary>
        /// The user who created the comment.
        /// </summary>
        public User Commenter { get; set; }

        /// <summary>
        /// The message (comment) associated with the object.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The time the comment was sent.
        /// </summary>
        public DateTime Time { get; set; }
    }
}