namespace ModelSaber.Models.Model
{
    public interface IModel : IModelSaberObject
    {
        string Name { get; }
        string[] Tags { get; }
        string Preview { get; }
        uint CollectionID { get; }
        DownloadFileType Type { get; }
    }
}