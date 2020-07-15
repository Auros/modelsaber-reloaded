using System;

namespace ModelSaber.Models
{
    public interface IModelSaberObject
    {
        uint ID { get; }
        DateTime Created { get; }
    }
}