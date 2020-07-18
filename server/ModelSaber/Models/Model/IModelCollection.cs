namespace ModelSaber.Models.Model
{
    public interface IModelCollection
    {
        string Name { get; }
        string Description { get; }
        string InstallPath { get; }
    }
}