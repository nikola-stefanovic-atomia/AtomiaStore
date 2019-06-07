using Atomia.Store.Core;
using System;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public class PackageProvider : IPackageProvider
    {
        private readonly PublicBillingApiProxy billingApi;

        public PackageProvider(PublicBillingApiProxy billingApi)
        {
            if (billingApi == null)
            {
                throw new ArgumentNullException(nameof(billingApi));
            }

            this.billingApi = billingApi;
        }

        public bool IsMultiPackageEnabled()
        {
            return billingApi.IsMultiPackageEnabled();
        }
    }
}
