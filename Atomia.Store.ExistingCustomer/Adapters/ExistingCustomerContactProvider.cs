using System.Web;
using Atomia.Store.Core;
using Atomia.Store.ExistingCustomer.Models;

namespace Atomia.Store.ExistingCustomer.Adapters
{
    public class ExistingCustomerContactProvider : IContactDataProvider
    {
        public void ClearContactData()
        {
            HttpContext.Current.Session["ExistingCustomerData"] = null;
        }

        public IContactDataCollection GetContactData()
        {
            var existingCustomerData = HttpContext.Current.Session["ExistingCustomerData"];

            if (existingCustomerData != null)
            {
                return (ExistingCustomerContactModel)existingCustomerData;
            }

            return null;
        }

        public void SaveContactData(IContactDataCollection contactData)
        {
            HttpContext.Current.Session["ExistingCustomerData"] = contactData;
        }
    }
}
