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
    public class ComputerVisionService : IComputerVisionService
    {
        #region variables

        private readonly AppSettings? _appSettings;
        #endregion

        public ComputerVisionService(IOptions<AppSettings> appSettings)
        {

            _appSettings = appSettings.Value;
        }

        public async Task<string> GenerateThumbnailAsync(AzureFileModel oAzureFileModel)
        {
            int size = _appSettings.ThumbSize;

            Image? oThumbnailImage;

            string sFileName = oAzureFileModel.FileName;
            string sFilePath = $"{BlobModel.ThumbnailPrefix}{sFileName}";

            using (var oComputerVisionClient = this.Authenticate())
            {
                using var oThumbStream = await oComputerVisionClient.GenerateThumbnailInStreamAsync(width: size, height: size, oAzureFileModel.Stream, smartCropping: true);

                using (var oMemoryStream = new MemoryStream())
                {
                    await oThumbStream.CopyToAsync(oMemoryStream);

                    oThumbnailImage = Image.FromStream(oMemoryStream);

                    oThumbnailImage.Save(sFilePath);

                }
            }

            return sFilePath;
        }



        public async Task<ImageAnalysis> AnalyzeImageUrl(string imageUrl)
        {

            using (var oComputerVisionClient = this.Authenticate())
            {
                List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>() {
                    VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                        VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                        VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                        VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                        VisualFeatureTypes.Objects
                };
                ImageAnalysis results = await oComputerVisionClient.AnalyzeImageAsync(imageUrl, visualFeatures: features);

                return results;
            }
        }
  

        private ComputerVisionClient Authenticate()
        {

            return new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(_appSettings.ComputerVisionKey))
            {
                Endpoint = _appSettings.ComputerVisionEndpoint
            };
        }

   
    }
}
