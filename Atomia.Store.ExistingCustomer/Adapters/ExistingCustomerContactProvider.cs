using System.Web;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;

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
