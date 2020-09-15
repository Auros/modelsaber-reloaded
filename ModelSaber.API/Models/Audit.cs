using ModelSaber.Common;
using System;

namespace ModelSaber.API.Models
{
    public class Audit
    {
        /// <summary>
        /// The ID of the Audit message
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The subject of the Audit
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The ID of the subject of the Audit
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The source of the audit message
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The action that is being audited.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The time when the action was audited.
        /// </summary>
        public DateTime Time { get; set; }
    }
}
