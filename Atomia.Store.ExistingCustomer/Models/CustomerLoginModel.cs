using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.Core;


namespace Atomia.Store.ExistingCustomer.Models
{
    public class CustomerLoginModel
    {
        /// <summary>
        /// Customer login username
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string Username { get; set; }

        /// <summary>
        /// Customer login password
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string Password { get; set; }
    }
}