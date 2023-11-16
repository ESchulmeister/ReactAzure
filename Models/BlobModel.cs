namespace reactAzure.Models
{
    public class BlobModel
    {
        public const string ThumbnailPrefix = "thumbnail_";

        public int ID { get; set; }


        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
        public string? ThumbnailUri { get; set; }

        public bool IsThumbnail
        {
            get
            {
                return String.IsNullOrWhiteSpace(this.Name) ? false : (this.Name.Contains(ThumbnailPrefix));
            }
        }
    }
}
