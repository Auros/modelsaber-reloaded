using Microsoft.AspNetCore.Http;

namespace ModelSaber.Models.Game
{
    public class UploadGame : IGame
    {
        public string Title { get; set; }
        public IFormFile Icon { get; set; }
        public string Description { get; set; }
        public Visibility Visibility { get; set; }
    }
}