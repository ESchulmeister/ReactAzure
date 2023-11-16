using reactAzure.Models;
using System.Drawing;

namespace reactAzure.Services
{
    public interface IAzureRepository
    {
        Task<List<BlobModel>> ListAsync();

        Task<BlobResponseModel> UploadAsync(AzureFileModel fileModel);

        Task<BlobResponseModel> UploadThumbAsync(FileInfo oThumbPathFileInfo);

   }
}
