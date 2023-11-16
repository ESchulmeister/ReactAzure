namespace reactAzure.Models
{
    public class AzureFileModel
    {
        public string? TempFilePath { get; set; }
        public Stream Stream { get; set; }

        public string FileName { get; set; }
    }
}
