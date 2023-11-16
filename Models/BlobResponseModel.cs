namespace reactAzure.Models
{
    public class BlobResponseModel
    {
 
        public BlobModel imageMetadata { get; set; }

        public BlobResponseModel()
        {
            imageMetadata = new BlobModel();
        }
    }
}
