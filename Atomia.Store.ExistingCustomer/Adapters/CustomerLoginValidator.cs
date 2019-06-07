using Atomia.Store.ExistingCustomer.Models;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.ExistingCustomer.Adapters
{
    public class CustomerLoginValidator
    {
        private AtomiaBillingPublicService service;

        public CustomerLoginValidator(AtomiaBillingPublicService service)
        {
            this.service = service;
        }

        public ExistingCustomerContactData ValidateCustomerLogin(string username, string password)
        {
            var validationResults = service.ValidateUser(username, password);

            return new ExistingCustomerContactData
            {
                Username = username,
                Password = password,
                Valid = validationResults.Status == "OK",
                CustomerNumber = validationResults.CustomerNumber ?? string.Empty,
                Country = validationResults.Address?.Country?.Code ?? string.Empty
            };
        }
    }
}
