using System;
using System.Collections.Generic;

namespace ModelSaber.Models.Model
{
    public class ModelCollection : IModelCollection
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public string IconURL{ get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string InstallPath { get; set; }
        public List<Model> Models { get; set; }
    }
}