using System;
using System.Collections.Generic;

namespace ModelSaber.Common
{
    public class Model
    {
        /// <summary>
        /// The ID of the model.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the model.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The MD5 hash of the model file.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// The tags associated with the model.
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// The uploader of the model.
        /// </summary>
        public User Uploader { get; set; }

        /// <summary>
        /// The type of file that the model is.
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// The Markdown description of the model.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The relative URL of the model of the file.
        /// </summary>
        public string DownloadURL { get; set; }

        /// <summary>
        /// The relative URL of the thumbnail image.
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// The time (UTC) when the model was uploaded.
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// The collection that thie model is present in.
        /// </summary>
        public Collection Collection { get; set; }

        /// <summary>
        /// The visibility of the model.
        /// </summary>
        public Visibility Visibility { get; set; }

        /// <summary>
        /// The current status of approval of the model.
        /// </summary>
        public ApprovalStatus Status { get; set; }

        /// <summary>
        /// Playlists that this model is present in.
        /// </summary>
        public ICollection<Playlist> Playlists { get; set; }
    }
}