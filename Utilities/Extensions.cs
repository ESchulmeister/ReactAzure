using Newtonsoft.Json.Linq;

namespace reactAzure
{
    public static class Extensions
    {

        #pragma warning disable CS8601 //possible null ref

        #region Methods

        /// <summary>
        /// Get FormData JSON Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sJson"></param>
        /// <param name="sKey"></param>
        /// <param name="oDefault"></param>
        /// <param name="oValue"></param>
        /// <returns></returns>
        public static bool GetJsonValue<T>(this string sJson, string sKey, T oDefault, out T oValue)
        {
            oValue = oDefault;

            JObject? oJObject = JObject.Parse(sJson);
            JToken? oValueJToken = oJObject.SelectToken(sKey);
            if (oValueJToken == null)
            {
                return false;
            }

            oValue = oValueJToken.Value<T>();

            return true;
        }

        /// <summary>
        /// User's identity - login credential
        /// </summary>
        /// <param name="oHttpContextAccessor"></param>
        /// <returns></returns>
        public static string? GetIdentity(this IHttpContextAccessor oHttpContextAccessor)
        {
            var currUser = oHttpContextAccessor.HttpContext!.User;

            return (currUser.Identity == null) ? string.Empty : currUser.Identity.Name;
        }


        public static byte[] ReadAllBytes(this Stream inStream)
        {
            if (inStream is MemoryStream)
                return ((MemoryStream)inStream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                inStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        #endregion
    }
}
