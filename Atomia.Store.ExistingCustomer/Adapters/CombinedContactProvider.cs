using Atomia.Store.AspNetMvc.Adapters;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using Atomia.Store.ExistingCustomer.Models;
using System;

namespace Atomia.Store.ExistingCustomer.Adapters
{
    public class CombinedContactProvider : IContactDataProvider
    {
        private ExistingCustomerContactProvider existingCustomerProvider = new ExistingCustomerContactProvider();
        private ContactDataProvider newCustomerProvider = new ContactDataProvider();

        public void ClearContactData()
        {
            existingCustomerProvider.ClearContactData();
            newCustomerProvider.ClearContactData();
        }

        public IContactDataCollection GetContactData()
        {
            var customerData = existingCustomerProvider.GetContactData();

            if (customerData == null)
            {
                customerData = newCustomerProvider.GetContactData();
            }

            return customerData;
        }

        public void SaveContactData(IContactDataCollection contactData)
        {
            var contactDataType = contactData.GetType();
            if (contactDataType == typeof(ExistingCustomerContactModel))
            {
                newCustomerProvider.ClearContactData();
                existingCustomerProvider.SaveContactData(contactData);
            }
            else if (contactDataType.IsSubclassOf(typeof(AccountViewModel)))
            {
                newCustomerProvider.SaveContactData(contactData);
            }
            else
            {
                throw new InvalidOperationException("Cannot save contact data of type " + contactDataType + ".");
            }
        }
    }
}
