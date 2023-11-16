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
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
//using System.Reflection.Metadata.Ecma335;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using System.Net;
using reactAzure.Models;
using reactAzure.Data;
using reactAzure.Services;
using ImageMagick;

namespace reactAzure.Controllers
{
    [ApiController]
    [Route("[controller]/{action}")]


    //  [Authorize]
    public class AzureController : ReactSampleController<AzureController>
    {

#pragma warning disable CS8604 //null ref

        #region variables 
        private readonly IAzureRepository _azureRepository;
        private readonly IComputerVisionService _computerVisionService;
        private readonly IOptions<AppSettings> _appSettings;
        #endregion


        public AzureController(IAzureRepository azureRepository, IComputerVisionService computerVisionService, IOptions<AppSettings> appSettings)
        {
            _azureRepository = azureRepository;
            _computerVisionService = computerVisionService;
            _appSettings = appSettings;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<BlobResponseModel> UploadFile( [FromForm] IFormFile file)
        {
            BlobResponseModel? oThumbnailBlobResponseModel = null;
            BlobResponseModel? oImageBlobResponseModel = null;
            AzureFileModel? oAzureFileModel = null;
            try
            {
                oAzureFileModel = new AzureFileModel()
                {
                    Stream = file.OpenReadStream(),
                    FileName = file.FileName,
                };

                this.ConvertToSupportedFormat(oAzureFileModel);

                oImageBlobResponseModel = await _azureRepository.UploadAsync(oAzureFileModel);


                if (oAzureFileModel.FileName.CanBeThumbnail())   /// NOT *.Png - svg converted files
                {

                    string sThumbImagePath = await _computerVisionService.GenerateThumbnailAsync(oAzureFileModel);

                    oThumbnailBlobResponseModel = await _azureRepository.UploadThumbAsync(new FileInfo(sThumbImagePath));

                    io.File.Delete(sThumbImagePath);

                }
                else
                {
                    oThumbnailBlobResponseModel = oImageBlobResponseModel;

                }

                return oThumbnailBlobResponseModel;
            }
            catch (Exception oException)
            {
                var errMsg = $"{oException.Message} \n @ {oException.StackTrace}";
                this.Logger.Error(errMsg);

                if(oThumbnailBlobResponseModel == null)
                {
                    oThumbnailBlobResponseModel = oImageBlobResponseModel;
                }

                return oThumbnailBlobResponseModel;

            }
            finally
            {
                if(oAzureFileModel != null && oAzureFileModel.Stream != null)
                {
                    oAzureFileModel.Stream.Close();
                    oAzureFileModel.Stream.Dispose();
                }

                if(!String.IsNullOrWhiteSpace(oAzureFileModel.TempFilePath))
                {
                    if(System.IO.File.Exists(oAzureFileModel.TempFilePath))
                    {
                        System.IO.File.Delete(oAzureFileModel.TempFilePath);
                    }
                }
            }
        }

        private void ConvertToSupportedFormat(AzureFileModel oAzureFileModel)
        {
            int iLastDot = oAzureFileModel.FileName.LastIndexOf(".");
            string sExtension = oAzureFileModel.FileName.Substring(iLastDot + 1).ToLower();

            switch (sExtension)
            {
                case Constants.ImageFormats.EPS:
                    {
                        var oEpsConverter = new Utilities.EpsConverter(_appSettings);
                        oAzureFileModel.TempFilePath = oEpsConverter.Convert(oAzureFileModel);
                        break;
                    }

                case Constants.ImageFormats.SVG:
                    {
                        var oSvgConverter = new Utilities.SvgConverter(_appSettings);
                        oAzureFileModel.TempFilePath = oSvgConverter.Convert(oAzureFileModel);
                        break;
                    }
                default: return;
            }

            byte[] aBytes = System.IO.File.ReadAllBytes(oAzureFileModel.TempFilePath);

            //Delete newly created Png file
            oAzureFileModel.Stream.Close();
            oAzureFileModel.Stream.Dispose();

            oAzureFileModel.Stream = new MemoryStream(aBytes);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlobModel>>> ListBlobs(DataSourceLoadOptions loadOptions)
        {

            try
            {
                List<BlobModel> lstFiles = await _azureRepository.ListAsync();

                return Ok(await Task.Run(() => DataSourceLoader.Load(lstFiles, loadOptions)));
            }

            catch (Exception oException)
            {
                var errMsg = $"{oException.Message} \n @ {oException.StackTrace}";
                this.Logger.Error(errMsg);
                return StatusCode((int)HttpStatusCode.InternalServerError, Constants.General_Error);
            }

        }

    }
}
