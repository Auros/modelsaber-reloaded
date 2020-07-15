using System.Collections.Generic;

namespace ModelSaber.Models.Model
{
    public interface IModelCollection : IModelSaberObject
    {
        string Name { get; }
        string IconURL { get; }
        string Description { get; }
        string InstallPath { get; }
        List<Model> Models { get; }
    }
}