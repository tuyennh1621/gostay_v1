using DAL.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class ConfigurationsHelper
    {
        public static ConcurrentDictionary<string, string> ConfigurationsDictionary = new ConcurrentDictionary<string, string>();

        public static void SetConfigurations(List<ConfigurationEntity> configurations)
        {
            if (configurations != null && configurations.Count > 0)
            {
                var _ConfigurationsDictionary = new ConcurrentDictionary<string, string>();

                foreach (var configuration in configurations)
                {
                    _ConfigurationsDictionary.TryAdd(configuration.Key, configuration.Value);
                }

                ConfigurationsDictionary = _ConfigurationsDictionary;
            }
        }

        public static T GetConfigValue<T>(string key)
        {
            
            if (string.IsNullOrEmpty(key) || !ConfigurationsDictionary.ContainsKey(key))
            {
                string s = "No such Configuration found key";
                return (T)Convert.ChangeType(s, typeof(T));
                 //throw (new ApplicationException("No such Configuration found: "));
            }
            try
            {
                var value = ConfigurationsDictionary[key];

                if (!string.IsNullOrEmpty(value))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    return default;
                }
            }  
            catch (Exception e)
            {
                throw (new ApplicationException(string.Format("Cannot convert Configuration value to {0}", typeof(T)), e));
            }
        }

        public static string ApplicationName
        {
            get
            {
                return (GetConfigValue<string>("ApplicationName"));
            }
        }

        public static string ApplicationIntro
        {
            get
            {
                return (GetConfigValue<string>("ApplicationIntro"));
            }
        }

        public static string AddressLine1
        {
            get
            {
                return (GetConfigValue<string>("AddressLine1"));
            }
        }

        public static string AddressLine2
        {
            get
            {
                return (GetConfigValue<string>("AddressLine2"));
            }
        }

        public static string PhoneNumber
        {
            get
            {
                return (GetConfigValue<string>("PhoneNumber"));
            }
        }

        public static string MobileNumber
        {
            get
            {
                return (GetConfigValue<string>("MobileNumber"));
            }
        }

        public static string Email
        {
            get
            {
                return (GetConfigValue<string>("Email"));
            }
        }

        public static string AdminEmailAddress
        {
            get
            {
                return (GetConfigValue<string>("AdminEmailAddress"));
            }
        }

        public static string FacebookURL
        {
            get
            {
                return (GetConfigValue<string>("FacebookURL"));
            }
        }

        public static string TwitterUsername
        {
            get
            {
                return (GetConfigValue<string>("TwitterUsername"));
            }
        }

        public static string TwitterURL
        {
            get
            {
                return (GetConfigValue<string>("TwitterURL"));
            }
        }

        public static string WhatsAppNumber
        {
            get
            {
                return (GetConfigValue<string>("WhatsAppNumber"));
            }
        }

        public static string InstagramURL
        {
            get
            {
                return (GetConfigValue<string>("InstagramURL"));
            }
        }

        public static string YouTubeURL
        {
            get
            {
                return (GetConfigValue<string>("YouTubeURL"));
            }
        }

        public static string LinkedInURL
        {
            get
            {
                return (GetConfigValue<string>("LinkedInURL"));
            }
        }

        public static string CurrencySymbol
        {
            get
            {
                return (GetConfigValue<string>("CurrencySymbol"));
            }
        }

        public static string PriceCurrencyPosition
        {
            get
            {
                return (GetConfigValue<string>("PriceCurrencyPosition"));
            }
        }

        public static int DigitsAfterDecimalPoint
        {
            get
            {
                return (GetConfigValue<int>("DigitsAfterDecimalPoint"));
            }
        }

        public static string ElasticEmail_APIKey
        {
            get
            {
                return (GetConfigValue<string>("ElasticEmail_APIKey"));
            }
        }

        public static string ElasticEmail_FromEmailAddress
        {
            get
            {
                return (GetConfigValue<string>("ElasticEmail_FromEmailAddress"));
            }
        }

        public static string ElasticEmail_FromEmailAddressName
        {
            get
            {
                return (GetConfigValue<string>("ElasticEmail_FromEmailAddressName"));
            }
        }

        public static string GoogleAnalyticsScript
        {
            get
            {
                return (GetConfigValue<string>("GoogleAnalyticsScript"));
            }
        }

        public static string FacebookMessengerScript
        {
            get
            {
                return (GetConfigValue<string>("FacebookMessengerScript"));
            }
        }

        public static string PostingRules
        {
            get
            {
                return (GetConfigValue<string>("PostingRules"));
            }
        }

        public static bool RequireAdminAprovalForPostedItems
        {
            get
            {
                return (GetConfigValue<bool>("RequireAdminAprovalForPostedItems"));
            }
        }

        public static string GoogleMapsKey
        {
            get
            {
                return (GetConfigValue<string>("GoogleMapsKey"));
            }
        }

        public static bool EnableCaching
        {
            get
            {
                return (GetConfigValue<bool>("EnableCaching"));
            }
        }

        public static string FacebookAuthenticationAppID
        {
            get
            {
                return (GetConfigValue<string>("FacebookAuthenticationAppID"));
            }
        }

        public static string FacebookAuthenticationAppSecret
        {
            get
            {
                return (GetConfigValue<string>("FacebookAuthenticationAppSecret"));
            }
        }

        public static string TwitterAuthenticationConsumerKey
        {
            get
            {
                return (GetConfigValue<string>("TwitterAuthenticationConsumerKey"));
            }
        }

        public static string TwitterAuthenticationConsumerSecret
        {
            get
            {
                return (GetConfigValue<string>("TwitterAuthenticationConsumerSecret"));
            }
        }

        public static string GoogleAuthenticationClientID
        {
            get
            {
                return (GetConfigValue<string>("GoogleAuthenticationClientID"));
            }
        }

        public static string GoogleAuthenticationClientSecret
        {
            get
            {
                return (GetConfigValue<string>("GoogleAuthenticationClientSecret"));
            }
        }
    }
}
