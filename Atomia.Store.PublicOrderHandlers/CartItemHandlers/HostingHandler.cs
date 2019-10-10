using Atomia.Store.PublicBillingApi.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System.Text.RegularExpressions;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    public class HostingHandler : OrderDataHandler
    {
        /// <summary>
        /// Handle items with hosting categories.
        /// </summary>
        public virtual IEnumerable<string> HandledCategories
        {
            get { return new[] { "HostingPackage", "DNS" }; }
        }

        /// <summary>
        /// Adds "Label" custom attribute to hosting items.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var hostingPackageItems = orderContext.ItemData.Where(i => this.HandledCategories.Intersect(i.Categories.Select(c => c.Name)).Count() > 0);

            var customData = new List<PublicOrderItemProperty>();
            var labelRegEx = new Regex(Common.RegularExpression.GetRegularExpressionForSubscriptionLabel());

            foreach (var item in hostingPackageItems)
            {
                var labelValue = GetLabel(item);
                if (!string.IsNullOrWhiteSpace(labelValue) && labelRegEx.IsMatch(labelValue))
                {
                    customData.Add(new PublicOrderItemProperty { Name = "Label", Value = labelValue });
                }

                Add(order, new PublicOrderItem
                {
                    ItemId = Guid.Empty,
                    ItemNumber = item.ArticleNumber,
                    RenewalPeriodId = item.RenewalPeriodId,
                    Quantity = item.Quantity,
                    CustomData = customData.ToArray()
                });
            }

            return order;
        }

        protected virtual string GetLabel(ItemData hostingPackageItems)
        {
            var label = hostingPackageItems.CartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == "Label");

            return label != null ? label.Value : string.Empty;
        }
    }
}
