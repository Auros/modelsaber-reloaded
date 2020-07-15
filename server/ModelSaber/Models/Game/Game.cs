using System;

namespace ModelSaber.Models.Game
{
    public class Game : IGame
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
    }
}