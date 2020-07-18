using System;

namespace ModelSaber.Models
{
    public interface IModelSaberObject
    {
        ulong ID { get; }
        DateTime Created { get; }
    }
}