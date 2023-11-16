namespace reactAzure.Models
{
    public class AppSettings
    {

        public string? Secret { get; set; }

        public string? StorageAccountContainer { get; set; }

        public string? ComputerVisionKey { get; set; }

        public string? ComputerVisionEndpoint { get; set; }

        public int ThumbSize { get; set; }

        public string? tempFolder { get; set; }

        public int DefaultSvgWidth { get; set; }
    }
}
