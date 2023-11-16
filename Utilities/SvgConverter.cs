using io = System.IO;
using ImageMagick;
using reactAzure.Models;
using reactAzure.Services;
using System.Drawing.Imaging;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DevExpress.Text.Interop;
using Svg;

namespace reactAzure.Utilities
{
    public class SvgConverter
    {

        #pragma warning disable CS8604 //possible null ref

        #region fields

        private readonly AppSettings? _appSettings;
        private readonly string? _tempFolder;
        private readonly int _DefaultSvgWidth;

        #endregion

        #region Constructors
        public SvgConverter(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            if(_appSettings == null) 
            {
                throw new MissingFieldException("Conversions", "App Settings");
            }

            _tempFolder = _appSettings.tempFolder;
            _DefaultSvgWidth = _appSettings.DefaultSvgWidth;
        }
        #endregion

        #region Methods

        public string Convert(AzureFileModel oAzureFileModel)
        {
            int width = 0, height = 0;

            System.IO.Directory.SetCurrentDirectory(_tempFolder);

            string[] parts = oAzureFileModel.FileName.Split(".");
            string sNewFileName = $"{parts[0]}.{Constants.ImageFormats.TargetFormat}";
            string sDestPath = $"{_tempFolder}{sNewFileName}";

            var svgDocument = SvgDocument.Open<SvgDocument>(oAzureFileModel.Stream);

            using (var smallBitmap = svgDocument.Draw())
            {
                width = smallBitmap.Width;
                height = smallBitmap.Height;
                if (width < _DefaultSvgWidth)
                {
                    width = _DefaultSvgWidth;
                    height = _DefaultSvgWidth / smallBitmap.Width * height;
                }
            }

            using (var newBitmap = svgDocument.Draw(width, height))
            {
                newBitmap.MakeTransparent();
                newBitmap.Save(sDestPath, Constants.ImageFormats.TargetFormat);

            }

            oAzureFileModel.FileName = sNewFileName;    // make sure the newly-created .png file is not eligible for thumbnail creation

            return sDestPath;

        }
        #endregion

    }
}
