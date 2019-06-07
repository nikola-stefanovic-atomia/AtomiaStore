using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Proxy for AtomiaBillingPublicService to be able to override for unit tests and such, 
    /// and for allowing use of more collection types.
    /// </summary>
    public class PublicBillingApiProxy
    {
        private AtomiaBillingPublicService service;

        public PublicBillingApiProxy(AtomiaBillingPublicService service)
        {
            this.service = service;
        }

        public AtomiaBillingPublicService Service
        {
            get
            {
                return service;
            }
        }

        public virtual PublicOrder CalculateOrder(PublicOrder publicOrder)
        {
            return service.CalculateOrder(publicOrder);
        }

        public virtual IEnumerable<Country> GetCountries()
        {
            return service.GetCountries();
        }

        public virtual AccountData GetAccountDataByHash(string accountHash)
        {
            return service.GetAccountDataByHash(accountHash);
        }

        public virtual AccountData GetResellerDataByUrl(string orderPageUrl)
        {
            return service.GetResellerDataByUrl(orderPageUrl);
        }

        public virtual AccountData GetDefaultResellerData()
        {
            return service.GetDefaultResellerData();
        }

        public virtual IEnumerable<AttributeData> CheckDomains(IEnumerable<string> domains)
        {
            var domainsArray = domains.ToArray();

            return service.CheckDomains(domainsArray);
        }

        public virtual PublicOrder CreateOrder(PublicOrder order)
        {
            return service.CreateOrder(order);
        }

        public virtual PublicPaymentTransaction MakePayment(PublicPaymentTransaction paymentTransaction)
        {
            return service.MakePayment(paymentTransaction);
        }

        public virtual PublicOrder CreateOrderWithLoginToken(PublicOrder order, string resellerRootDomain, out string token)
        {
            return service.CreateOrderWithLoginToken(order, resellerRootDomain, out token);
        }

        public virtual VatNumberValidationResultType ValidateVatNumber(string countryCode, string vatNumber)
        {
            return service.ValidateVatNumber(countryCode, vatNumber);
        }

        public virtual bool IsMultiPackageEnabled()
        {
            return service.IsMultiPackageEnabled();
        }
    }
}
