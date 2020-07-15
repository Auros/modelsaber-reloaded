namespace ModelSaber.Models.Game
{
    public interface IGame : IModelSaberObject
    {
        string Title { get; }
        string Description { get; }
    }
}