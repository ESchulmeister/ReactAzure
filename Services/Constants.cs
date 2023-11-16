using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing.Imaging;

namespace reactAzure.Services
{
    public class Constants
    {

       public const string AuthCookie = "_authReact";

        public const int CacheExpHrs = 24;

        public const long MaxBlobSize = 1048576;

        public const string General_Error = "An Unexpected  error has occurred. Please contact the system administrator.";
        public const string Bad_Request_Error = "Invalid Request.";

        public static class AntiForgery
        {
            public const string Header = "X-XSRF-TOKEN";
            public const string Cookie = "XSRF-TOKEN";
        }

        public static class CacheKeys
        {
            public const string Roles = "_cacheRoles";
            public const string Periods = "_cachePeriods";
            public const string Managers = "_cacheManagers";
            public const string Projects = "_cacheProjects";

        }

        public static class LdapAttributes
        {
            public const string CommonName = "cn";
            public const string LastName = "sn";
            public const string FirstName = "givenname";
        }

        public static class Roles
        {
            public const int Administator = 1;
            public const int Supervisor = 2;
        }


        public static class ImageFormats
        {
            public static ImageFormat TargetFormat = ImageFormat.Png;
            public const string EPS = "eps";
            public const string SVG = "svg";

        }


    }

}
