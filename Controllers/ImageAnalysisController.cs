using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage;
using Azure.Storage.Sas;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Identity;
using System.Text;
using System.Drawing;
using io = System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Net;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using reactAzure.Models;
using reactAzure.Services;
using reactAzure.Data;

namespace reactAzure.Controllers
{
    [ApiController]
    [Route("[controller]/{action}")]

   // [Authorize]
    public class ImageAnalysisController : ReactSampleController<AzureController>
    {

#pragma warning disable CS8604 //null ref

        #region variables 

        private readonly IComputerVisionService _computerVisionService;

        #endregion

        public ImageAnalysisController(IComputerVisionService computerVisionService)
        {
            _computerVisionService= computerVisionService;
        }

        [HttpGet]
        public async Task<ImageAnalysisViewModel> GetMetadata([FromQuery] string imageUrl)
        {

            try
            {               
                ImageAnalysisViewModel oImageAnalysisViewModel = new ImageAnalysisViewModel();

                oImageAnalysisViewModel.Metadata = await _computerVisionService.AnalyzeImageUrl(imageUrl);

                return oImageAnalysisViewModel;

           }

            catch (Exception oException)
            {
                var errMsg = $"{oException.Message} \n @ {oException.StackTrace}";
                this.Logger.Error(errMsg);

                return null;

            }

        }

    }
}
