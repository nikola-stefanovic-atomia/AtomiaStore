using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Represents a reCaptcha validation api response
    /// </summary>
    [DataContract]
    public class RecaptchaResponse
    {
        /// <summary>
        /// Indicates wheter reCaptcha validation was successfull
        /// </summary>
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Optional
        /// </summary>
        [DataMember(Name = "error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
