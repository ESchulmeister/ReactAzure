using io = System.IO;
using ImageMagick;
using reactAzure.Models;
using reactAzure.Services;
using System.Drawing.Imaging;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace reactAzure.Utilities
{
    public class EpsConverter
    {
        #region fields

        private  readonly AppSettings? _appSettings;
        private readonly string? _tempFolder;


        #endregion

        #region Constructors
        public EpsConverter(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            if(_appSettings == null) 
            {
                throw new MissingFieldException("Conversions", "App Settings");
            }

            _tempFolder = _appSettings.tempFolder;
        }
        #endregion

        #region Methods

        public string Convert(AzureFileModel oAzureFileModel)
        {
            byte[]? bytes = oAzureFileModel.Stream.ReadAllBytes();
            string[] parts = oAzureFileModel.FileName.Split(".");
            string sNewFileName = $"{parts[0]}.{Constants.ImageFormats.TargetFormat}";
            string sDestPath = $"{_tempFolder}{sNewFileName}";

            using (var oMagickImage = new MagickImage(bytes, MagickFormat.Eps))
            {
                oMagickImage.Write(sDestPath);
            }

            oAzureFileModel.FileName = sNewFileName;    // make sure the newly-created .png file is not eligible for thumbnail creation

            return sDestPath;

        }
        #endregion

    }
}
