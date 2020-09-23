using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModelSaber.Common
{
    public class Playlist
    {
        /// <summary>
        /// The ID of the playlist.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The creator of the playlist.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The name of the playlist.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Markdown description of the playlist.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The relative URL of the thumbnail image.
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// The models in this playlist.
        /// </summary>
        public ICollection<Model> Models { get; set; } = new Collection<Model>();
    }
}