using Newtonsoft.Json.Linq;

namespace reactAzure.Services
{
    public static  class Extensions
    {

#pragma warning disable CS8601 //possible null ref


        public static bool GetJsonValue<T>(this string sJson, string sKey, T oDefault, out T oValue)
        {
            oValue = oDefault;

            JObject? oJObject = JObject.Parse(sJson);
            JToken? oValueJToken = oJObject.SelectToken(sKey);
            if (oValueJToken == null)
            {
                return false;
            }

            oValue =   oValueJToken.Value<T>();

            return true;
        }

        public static bool CanBeThumbnail(this FileInfo oFileInfo)
        {
            return oFileInfo.Name.CanBeThumbnail();

        }
        public static bool CanBeThumbnail(this string sFileName)
        {
            string[] NoThumbnailExtensions = new string[] { ".png" };

            return !NoThumbnailExtensions.Any(sExtension => sFileName.ToLower().IndexOf(sExtension) >= 0);

        }
    }
}
