﻿using System;
using ModelSaber.Common;

namespace ModelSaber.API.Models
{
    public class Audit
    {
        /// <summary>
        /// The ID of the Audit message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The subject of the Audit.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The subject of the audit message.
        /// </summary>
        public Guid Subject { get; set; }

        /// <summary>
        /// The source of the audit message.
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