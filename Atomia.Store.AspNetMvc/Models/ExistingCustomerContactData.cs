using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    public class ExistingCustomerContactData : ContactData
    {
        public override string Id
        {
            get { return "ExistingCustomerContact"; }
        }

        public override string Country { get; set; }
        
        public bool Valid { get; set; }

        [AtomiaRequired("Common,ErrorEmptyField")]
        public string Username { get; set; }

        [AtomiaRequired("Common,ErrorEmptyField")]
        public string Password { get; set; }

        public string CustomerNumber { get; set; }
    }
}
