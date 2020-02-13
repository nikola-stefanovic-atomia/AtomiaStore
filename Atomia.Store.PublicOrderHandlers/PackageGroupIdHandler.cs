using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using Atomia.Store.Core;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to amend order with the package group id when the multipackage is enabled.
    /// </summary>
    public class PackageGroupIdHandler : OrderDataHandler
    {
        /// <summary>
        /// The package provider instance
        /// </summary>
        private readonly IPackageProvider _packageProvider;

        /// <summary>
        /// Create a new instance of the handler.
        /// </summary>
        /// <param name="packageProvider">The package provider instance.</param>
        public PackageGroupIdHandler(IPackageProvider packageProvider)
        {
            if (packageProvider == null)
            {
                throw new ArgumentNullException(nameof(IPackageProvider));
            }

            _packageProvider = packageProvider;
        }

        /// <summary>
        /// Amends the order with the package group id whene the multipackage is enabled
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            if (_packageProvider.IsMultiPackageEnabled())
            {
                var packageGroupId = Guid.NewGuid().ToString();
                foreach (var item in order.OrderItems)
                {
                    var packageGroupIdArray = new PublicOrderItemProperty[1]
                    {
                        new PublicOrderItemProperty
                        {
                            Name = "PackageGroupId",
                            Value = packageGroupId
                        }
                    };

                    if (item.CustomData == null)
                    {
                        item.CustomData = packageGroupIdArray;
                        continue;
                    }

                    item.CustomData = item.CustomData.Concat(packageGroupIdArray).ToArray();
                }
            }

            return order;
        }
    }
}
