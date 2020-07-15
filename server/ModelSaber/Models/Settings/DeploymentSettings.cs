namespace ModelSaber.Models.Settings
{
    public class DeploymentSettings : IDeploymentSettings
    {
        public string[] CORS { get; set; }
    }

    public interface IDeploymentSettings
    {
        string[] CORS { get; set; }
    }
}