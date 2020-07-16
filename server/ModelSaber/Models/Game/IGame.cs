namespace ModelSaber.Models.Game
{
    public interface IGame
    {
        string Title { get; }
        string Description { get; }
        Visibility Visibility { get; set; }
    }
}