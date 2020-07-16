﻿using System;

namespace ModelSaber.Models.Game
{
    public class Game : IGame, IModelSaberObject
    {
        public uint ID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
    }
}