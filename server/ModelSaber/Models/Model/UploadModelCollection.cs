using Microsoft.AspNetCore.Http;

namespace ModelSaber.Models.Model
{
    public class UploadModelCollection : IModelCollection
    {
        public string Name { get; set; }
        public IFormFile Icon { get; set; }
        public string Description { get; set; }
        public string InstallPath { get; set; }
        public string FileExtension { get; set; }
        public Visibility Visibility { get; set; }

        public uint GameID { get; set; }
    }
}