using System;

namespace ModelSaber.Models.Model
{
    public class Model : IModel
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string[] Tags { get; set; }
        public string Preview { get; set; }
        public DateTime Created { get; set; }
        public uint CollectionID { get; set; }
        public string InstallURL { get; set; }
        public string DownloadURL { get; set; }
        public DownloadFileType Type { get; set; }
        public ModelCollection Collection { get; set; }

        public bool IsVariation { get; set; }
        public uint[] Variations { get; set; }
    }
}