using Atomia.Store.AspNetMvc.Models;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace Atomia.Store.AspNetMvc.Helpers
{
    /// <summary>
    /// The recaptcha helper.
    /// </summary>
    public static class RecaptchaHelper
    {
        /// <summary>
        /// Checks if the recaptcha response is valid.
        /// </summary>
        /// <param name="response">The recaptcha response.</param>
        /// <returns></returns>
        public static bool IsResponseValid(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return false;
            }

            var secret = ConfigurationManager.AppSettings["RecaptchaSecretKey"];
            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={response}";

            using (var client = new HttpClient())
            {
                var stream = client.GetStreamAsync(url).GetAwaiter().GetResult();
                using (var streamReader = new StreamReader(stream))
                {
                    var streamResponse = streamReader.ReadToEnd();
                    var javaScriptSerilizer = new JavaScriptSerializer();
                    var data = javaScriptSerilizer.Deserialize<RecaptchaResponse>(streamResponse);

                    return data.Success;
                }
            }
        }

        /// <summary>
        /// Checks if the recaptcha is enabled.
        /// </summary>
        /// <returns></returns>
        public static bool IsRecaptchaEnabled()
        {
            bool recaptchaEnabledParsed = false;

            bool.TryParse(ConfigurationManager.AppSettings["RecaptchaEnabled"], out recaptchaEnabledParsed);

            return recaptchaEnabledParsed;
        }

        /// <summary>
        /// Gets recaptcha site key from App settings.
        /// </summary>
        /// <returns></returns>
        public static string GetSiteKey()
        {
            return ConfigurationManager.AppSettings["RecaptchaSiteKey"];
        }
    }
}
