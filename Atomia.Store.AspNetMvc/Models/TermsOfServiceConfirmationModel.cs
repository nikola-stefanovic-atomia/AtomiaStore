using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for collecting customer confirmation of terms of service.
    /// </summary>
    public class TermsOfServiceConfirmationModel
    {
        /// <summary>
        /// The unique id of the terms of service.
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string Id { get; set; }

        /// <summary>
        /// Localized name of the terms of service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer provided confirmation for the terms of service.
        /// </summary>
        [AtomiaConfirmation("Common,ErrorTermNotChecked")]
        public bool Confirm { get; set; }
    }
}
