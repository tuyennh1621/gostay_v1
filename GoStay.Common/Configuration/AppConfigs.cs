using GoStay.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;

namespace GoStay.Common.Configuration
{
    public class AppConfigs
    {
        private static IConfiguration _configuration;

        public static string localSessionID="localSessionID";

        public static void LoadAll(IConfiguration configuration)
        {
            _configuration = configuration;
            ApiUrlBase = GetConfigValue("Appconfig:ApiUrlBase", "");
            ApiAir = GetConfigValue("Appconfig:ApiAir", "");
            ItemPerPage = GetConfigValue("Appconfig:ItemPerPage", 50);

            FestivallNews = GetConfigValue("Appconfig:FestivallNews", 1);
            FoodNews = GetConfigValue("Appconfig:FoodNews", 2);
            SaleslNews = GetConfigValue("Appconfig:SaleslNews", 5);
            CultureNews = GetConfigValue("Appconfig:CultureNews", 3);
            InterviewNews = GetConfigValue("Appconfig:InterviewNews", 4);
            TravelNews = GetConfigValue("Appconfig:TravelNews", 6);
            SqlConnection = GetConfigValue("ConnectionStrings:OnlineQuizConn", "No connection");
            GoogleClientId = GetConfigValue("Google:ClientId", "No value");
            GoogleClientSecret = GetConfigValue("Google:ClientSecret", "No value");
            FacebookClientId = GetConfigValue("Facebook:ClientId", "No value");
            FacebookClientSecret = GetConfigValue("Facebook:ClientSecret", "No value");

            NewsImagePath = GetConfigValue("Appconfig:NewsImagePath", "wwwroot\\shared\\UserFiles");
            FullPath = GetConfigValue("Appconfig:FullPath", "wwwroot\\upload\\");
            Directorio = GetConfigValue("Appconfig:Directorio", "wwwroot\\upload\\");
            SecretKey = GetConfigValue("Appconfig:SecretKey", "");
            AppVersion = GetConfigValue("Appconfig:AppVersion", "");
            IdDomain = GetConfigValue("Appconfig:IdDomain", "");
            Domain = GetConfigValue("Appconfig:Domain", "");
            AppotaKey = GetConfigValue("Appconfig:AppotaKey", "");
            AppotaApi = GetConfigValue("Appconfig:AppotaApi", "");
            AppotaIssuer = GetConfigValue("Appconfig:AppotaIssuer", "");
            AppotaClienIp = GetConfigValue("Appconfig:AppotaClienIp", "");
            AppotaEndPoint = GetConfigValue("Appconfig:AppotaEndPoint", "");
            //MoMo config
            MoMoEndPoint = GetConfigValue("Appconfig:MoMoEndPoint", "");
            MoMoPartnerCode = GetConfigValue("Appconfig:MoMoPartnerCode", "");
            MoMoAccessKey = GetConfigValue("Appconfig:MoMoAccessKey", "");
            MoMoSerectKey = GetConfigValue("Appconfig:MoMoSerectKey", "");
            MoMoIpnUrl = GetConfigValue("Appconfig:MoMoIpnUrl", "");
            MoMoNotifyurl = GetConfigValue("Appconfig:MoMoNotifyurl", "");
            MoMoRequestType = GetConfigValue("Appconfig:MoMoRequestType", "");
            MoMoLang = GetConfigValue("Appconfig:MoMoLang", "");
            VNPSerectKey = GetConfigValue("Appconfig:VNPSerectKey", "");
            VNPPayment = GetConfigValue("Appconfig:VNPPayment", "");
            VNPMerchant = GetConfigValue("Appconfig:VNPMerchant", "");
            VNPCode = GetConfigValue("Appconfig:VNPCode", "");
            VNPApiUrl = GetConfigValue("Appconfig:VNPApiUrl", "");
            VNPLang = GetConfigValue("Appconfig:VNPLang", "");
            //VNPay

            LogPath = GetConfigValue("Appconfig:LogPath", "");

        }

        public static string GetNameByPoint(double point)
        {
            var nameRating = "Tuyệt vời";
            if (point >= 0 && point < 3) nameRating = "Kém";
            else if (point >= 3 && point < 7) nameRating = "Trung bình";
            else if (point >= 7 && point < 9) nameRating = "Tốt";
            else if (point >= 9 && point <= 10) nameRating = "Tuyệt vời";

            return nameRating;
        }
        public static string FormatFar(float Howfar)
        {
            return Math.Round(Math.Round(Howfar*1.2, 0)/1000,1) + " (km)";
        }
        public static string FormatCurrency(string currencyCode, decimal amount)
        {
            CultureInfo culture = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                   let r = new RegionInfo(c.LCID)
                                   where r != null
                                   && r.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper()
                                   select c).FirstOrDefault();
            if (culture == null)
            {
                // fall back to current culture if none is found
                // you could throw an exception here if that's not supposed to happen
                culture = CultureInfo.CurrentCulture;

            }
            culture = (CultureInfo)culture.Clone();
            culture.NumberFormat.CurrencySymbol = currencyCode;
            culture.NumberFormat.CurrencyPositivePattern = culture.NumberFormat.CurrencyPositivePattern == 0 ? 2 : 3;
            var cnp = culture.NumberFormat.CurrencyNegativePattern;
            switch (cnp)
            {
                case 0: cnp = 14; break;
                case 1: cnp = 9; break;
                case 2: cnp = 12; break;
                case 3: cnp = 11; break;
                case 4: cnp = 15; break;
                case 5: cnp = 8; break;
                case 6: cnp = 13; break;
                case 7: cnp = 10; break;
            }
            culture.NumberFormat.CurrencyNegativePattern = cnp;

            return amount.ToString("C" + ((amount % 1) == 0 ? "0" : "2"), culture);
        }
        const string BuildVersionMetadataPrefix = "+build";
        const string dateFormat = "yyyy-MM-ddTHH:mm:ss:fffZ";


        public static DateTime GetLinkerTime(Assembly assembly)
        {
            var attribute = assembly
              .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value[(index + BuildVersionMetadataPrefix.Length)..];

                    return DateTime.ParseExact(
                        value,
                      dateFormat,
                      CultureInfo.InvariantCulture);
                }
            }
            return default;
        }

        public static string buidinfo()
        {
            return "Gostay 3.0 - latest build: " + GetLinkerTime(Assembly.GetEntryAssembly()) + " (beta version).";
        }
        public static string checkinDate = DateTime.Now.ToString("dd/MM/yyyy");
        public static string StartDateTours = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
        public static string checkoutDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
        //public static string OrderCode = DateTime.UtcNow.TimeOfDay.TotalSeconds.ToString();
        public static DateTime startDate = new DateTime(2022, 1, 1);
        public static string ApiUrlBase { get; set; }
        public static string ApiAir { get; set; }

        public static string GoogleClientId { get; set; }
        public static string GoogleClientSecret { get; set; }
        public static string FacebookClientId { get; set; }
        public static string FacebookClientSecret { get; set; }
        public static int ItemPerPage { get; set; }
        public static int FestivallNews { get; set; }
        public static int FoodNews { get; set; }
        public static int SaleslNews { get; set; }
        public static int CultureNews { get; set; }
        public static int InterviewNews { get; set; }
        public static int TravelNews { get; set; }
        public static string SqlConnection { get; set; }

        public static string NewsImagePath { get; set; }
        public static string FullPath { get; set; }
        public static string Directorio { get; set; }
        public static string SecretKey { get; set; }
        public static string AppVersion { get; set; }
        public static string IdDomain { get; set; }
        public static string Domain { get; set; }
        public static string AppotaKey { get; set; }
        public static string AppotaApi { get; set; }
        public static string AppotaClienIp { get; set; }
        public static string AppotaEndPoint { get; set; }
        public static string AppotaIssuer { get; set; }

        //MoMo config
        public static string MoMoEndPoint { get; set; }
        public static string MoMoPartnerCode { get; set; }
        public static string MoMoAccessKey { get; set; }
        public static string MoMoSerectKey { get; set; }
        public static string MoMoIpnUrl { get; set; }
        public static string MoMoNotifyurl { get; set; }
        public static string MoMoRequestType { get; set; }
        public static string MoMoLang { get; set; }
        public static string LogPath { get; set; }
        public static string VNPSerectKey { get; set; }
        public static string VNPPayment { get; set; }
        public static string VNPMerchant { get; set; }
        public static string VNPCode { get; set; }
        public static string VNPApiUrl { get; set; }
        public static string VNPLang { get; set; }


        public static int NumRoom = 1;
        public static int NumAdult = 1;
        public static int NumChildren = 0;
        public static string TextHotelSearchBar = "Chọn số phòng, số khách";

        public static int Adult = 1;
        public static int Children = 0;
        public static int Infant = 0;
        public static string DepartureDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
        public static string ReturnDate = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");

        public static string TextTicketSearchBar = "Chọn số hành khách";

        public static string CurrentUserCK = "CurrentUser";
        public static string AnonyUser = "AnonyUser";
        public static string CurrentUserAdmin = "CurrentUserAdmin";
        public static string CTNAME = "Việt Nam";
        public static string strTitle = "Gostay - Nền tảng thương mại điện tử cho du lịch hàng đầu Việt Nam";
        /// <summary>
        /// plhd
        /// </summary>
        /// 
        public static int permission = -1;
        public static string pls(string _str)
        {
            return "Bạn cần nhập vào " + _str;
        }

        public static int acvivemenu = 0;
        /// <summary>
        /// Lấy ra giá trị config trong file .config
        /// </summary>
        private static T GetConfigValue<T>(string configKey, T defaultValue)
        {
            var value = defaultValue;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                if (converter != null)
                {
                    var setting = _configuration.GetSection(configKey).Value;
                    if (!string.IsNullOrEmpty(setting))
                    {
                        value = (T)converter.ConvertFromString(setting);
                    }
                }
            }
            catch
            {
                value = defaultValue;
            }
            return value;
        }

        public static string Activecss(int tab, int tabactive)
        {
            if (tab == tabactive)
            {
                return "active";
            }
            else
            {
                return "";
            }
        }
    }
}
