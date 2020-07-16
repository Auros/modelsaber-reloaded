namespace ModelSaber.Models.Model
{
    public interface IModelCollection : IModelSaberObject
    {
        string Name { get; }
        string Description { get; }
        string InstallPath { get; }
    }
}