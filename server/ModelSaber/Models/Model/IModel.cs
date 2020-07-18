using System.Collections.Generic;

namespace ModelSaber.Models.Model
{
    public interface IModel
    {
        string Name { get; }
        string Preview { get; }
        ulong CollectionID { get; }
        List<string> Tags { get; }
        DownloadFileType Type { get; }
    }
}