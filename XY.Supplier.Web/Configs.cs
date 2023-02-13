using System.IO;
using Microsoft.Extensions.Configuration;

namespace XY.Supplier.Web
{
    public static class Configs
    {
        public static string NsapBone = string.Empty;
        public static string SiteDomain = string.Empty;
        public static string SiteTitle = string.Empty;
        public static string EncryptKey = string.Empty;
        public static string EncryptIV = string.Empty;
        public static string SignKey = string.Empty;
        public static string BossSiteURL = string.Empty;
        public static string FileSiteURL = string.Empty;
        public static string ProductSiteURL = string.Empty;
        public static string BlogSiteURL = string.Empty;
        public static string QrLoginURL = string.Empty;
        public static string XWZNClientID = string.Empty;
        public static string XWZNSecret = string.Empty;
        public static string XWZNScope = string.Empty;

        /// <summary>
        /// 企业ID
        /// </summary>
        public static int EnterpriseID = 1;

        static Configs()
        {
            IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            SiteDomain = config["AppSettings:SiteDomain"];
            SiteTitle = config["AppSettings:SiteTitle"];
            EncryptKey = config["AppSettings:EncryptKey"];
            EncryptIV = config["AppSettings:EncryptIV"];
            SignKey = config["AppSettings:SignKey"];
            BossSiteURL = config["AppSettings:BossSiteURL"];
            FileSiteURL = config["AppSettings:FileSiteURL"];
            ProductSiteURL = config["AppSettings:ProductSiteURL"];
            BlogSiteURL = config["AppSettings:BlogSiteURL"];

            EnterpriseID = int.Parse(config["AppSettings:EnterpriseID"]);
            QrLoginURL = config["AppSettings:QrLoginURL"];
            XWZNClientID = config["AppSettings:XWZNClientID"];
            XWZNSecret = config["AppSettings:XWZNSecret"];
            XWZNScope = config["AppSettings:XWZNScope"];
            NsapBone = config["ConnectionStrings:NsapBone"];
        }
    }
}