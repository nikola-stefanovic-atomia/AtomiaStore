using System.Configuration;
using System.Linq;
using System.Web;

namespace Atomia.Store.AspNetMvc.Helpers
{
    public static class ConfigurationHelper
    {
        public static string ReadConfigurationOption(string configurationKey, string fallbackValue = null)
        {
            if (HttpContext.Current.Application.AllKeys.Contains(configurationKey))
            {
                var value = HttpContext.Current.Application[configurationKey].ToString();
                return value;
            }
            return fallbackValue;
        }

        public static string GetDnsPackageArticleNumber()
        {
            return ReadConfigurationOption("DnsPackageArticleNumber", "DNS-PK");
        }

        public static bool AllowExistingCustomerOrders()
        {
            var allowExistingCustomerOrdersSetting = ConfigurationManager.AppSettings["AllowExistingCustomerOrders"] as string;
            bool retVal;

            if (!bool.TryParse(allowExistingCustomerOrdersSetting, out retVal))
            {
                throw new ConfigurationErrorsException("Could not parse boolean from 'AllowExistingCustomerOrders' setting or it is missing.");
            }

            return retVal;
        }

        public static bool IsPackageLabelEnabled()
        {
            var enablePckageLabelSetting = ConfigurationManager.AppSettings["EnablePackageLabel"] as string;
            bool retVal;

            if (!bool.TryParse(enablePckageLabelSetting, out retVal))
            {
                throw new ConfigurationErrorsException("Could not parse boolean from 'EnablePackageLabel' setting or it is missing.");
            }

            return retVal;
        }
    }
}
