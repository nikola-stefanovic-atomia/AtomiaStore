using Atomia.Store.Core;

namespace Atomia.Store.ExistingCustomer.Models
{
    public class ExistingCustomerContactData : ContactData
    {
        public override string Id
        {
            get { return "ExistingCustomerContact"; }
        }

        public override string Country { get; set; }

        public bool Valid { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CustomerNumber { get; set; }
    }
}
