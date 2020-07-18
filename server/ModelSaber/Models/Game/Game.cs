using ModelSaber.Models.Model;
using System;
using System.Collections.Generic;

namespace ModelSaber.Models.Game
{
    public class Game : IGame, IModelSaberObject
    {
        public ulong ID { get; set; }
        public string Title { get; set; }
        public string IconURL { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
        public List<ModelCollection> Collections { get; set; } = new List<ModelCollection>();
    }
}