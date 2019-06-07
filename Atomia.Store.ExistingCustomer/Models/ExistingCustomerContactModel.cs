using System.Collections.Generic;
using Atomia.Store.Core;

namespace Atomia.Store.ExistingCustomer.Models
{
    public class ExistingCustomerContactModel : IContactDataCollection
    {
        private ExistingCustomerContactData contactData;

        public IEnumerable<ContactData> GetContactData()
        {
            if (this.contactData != null)
            {
                return new List<ContactData> { this.contactData };
            }

            return null;
        }

        public void SetContactData(ExistingCustomerContactData contactData)
        {
            this.contactData = contactData;
        }
    }
}
