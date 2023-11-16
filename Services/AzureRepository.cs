using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using reactAzure.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace reactAzure.Services
{
    public class AzureRepository : IAzureRepository
    {
        #region variables
        private readonly BlobServiceClient? _blobServiceClient;
        private readonly string? _storageContainer;
        private readonly AppSettings? _appSettings;
        #endregion

        public AzureRepository(BlobServiceClient blobServiceClient, IOptions<AppSettings> appSettings)
        {

            _blobServiceClient = blobServiceClient;
            _appSettings = appSettings.Value;
            _storageContainer = _appSettings.StorageAccountContainer;
        }


        public async Task<BlobResponseModel> UploadAsync(AzureFileModel oAzureFileModel)
        {
            BlobResponseModel response = new();

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_storageContainer);

            var blobClient = blobContainer.GetBlobClient(oAzureFileModel.FileName);
            await blobClient.UploadAsync(oAzureFileModel.Stream, overwrite: true);

            response.imageMetadata.Uri = blobClient.Uri.AbsoluteUri;
            response.imageMetadata.Name = blobClient.Name;

            return response;
        }

        public async Task<BlobResponseModel> UploadThumbAsync(FileInfo oThumbPathFileInfo)
        {
            
            BlobResponseModel response = new();

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_storageContainer);

            var blobClient = blobContainer.GetBlobClient(oThumbPathFileInfo.Name);

            using (Stream? oStream = File.OpenRead(oThumbPathFileInfo.FullName))
            {

                await blobClient.UploadAsync(oStream, overwrite: true);
                oStream.Close();
            }

            response.imageMetadata.Uri = blobClient.Uri.AbsoluteUri;
            response.imageMetadata.Name = blobClient.Name;

            return response;
        }

        public async Task<List<BlobModel>> ListAsync()
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_storageContainer);

            var lstFiles = new List<BlobModel>();

            int iID = 0;

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                var thumbnailUri = name.CanBeThumbnail() ? $"{uri}/{BlobModel.ThumbnailPrefix}{name}" : fullUri;

                var oBlobModel = new BlobModel
                {
                    ID = ++iID,
                    Uri = fullUri,
                    ThumbnailUri = thumbnailUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                };

                if(!oBlobModel.IsThumbnail)
                {
                    lstFiles.Add(oBlobModel);
                }
            }

            return lstFiles;
        }

    }


}
