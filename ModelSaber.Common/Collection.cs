using System;

namespace ModelSaber.Common
{
    public class Collection
    {
        /// <summary>
        /// The ID of the collection.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the collection.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The relative URL of the icon image.
        /// </summary>
        public string IconURL { get; set; }

        /// <summary>
        /// The description of the collection.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The default install path for models in the collection.
        /// </summary>
        public string DefaultInstallPath { get; set; }

        /// <summary>
        /// The default visibility for models uploaded in this collection.
        /// </summary>
        public Visibility DefaultVisibility { get; set; }

        /// <summary>
        /// The default approval status for models uploaded in this collection.
        /// </summary>
        public ApprovalStatus DefaultApprovalStatus { get; set; }
    }
}