using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using reactAzure.Models;

namespace reactAzure.Services
{
    public interface IComputerVisionService
    {

        Task<string> GenerateThumbnailAsync(AzureFileModel model);

        Task<ImageAnalysis> AnalyzeImageUrl(string imageUrl);
    }
}
