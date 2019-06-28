using System.Collections.Generic;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
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
