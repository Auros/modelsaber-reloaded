namespace ModelSaber.Models.Settings
{
    public class DiscordSettings : IDiscordSettings
    {
        public string ID { get; set; }
        public string Secret { get; set; }
        public string RedirectURL { get; set; }
    }

    public interface IDiscordSettings
    {
        string ID { get; set; }
        string Secret { get; set; }
        string RedirectURL { get; set; }
    }
}