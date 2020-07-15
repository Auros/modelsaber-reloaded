namespace ModelSaber.Models.Model
{
    public interface IModel : IModelSaberObject
    {
        string Name { get; }
        string Hash { get; }
        string[] Tags { get; }
        string Preview { get; }
        uint CollectionID { get; }
        string InstallURL { get; }
        string DownloadURL { get; }
        DownloadFileType Type { get; }
        ModelCollection Collection { get; }
    }
}