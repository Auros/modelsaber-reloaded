using System;
using System.Collections.Generic;

namespace ModelSaber.Models.Model
{
    public class Model : IModel
    {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Preview { get; set; }
        public DateTime Created { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string InstallURL { get; set; }
        public ulong CollectionID { get; set; }
        public string DownloadURL { get; set; }
        public DownloadFileType Type { get; set; }
        public Visibility Visibility { get; set; }
        public ModelCollection Collection { get; set; }

        public ulong? VariationOfID { get; set; }
        public Model VariationOf { get; set; }

        public ModelStats Stats { get; set; } = new ModelStats();
    }
}