using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicOrderHandlers.CartItemHandlers
{
    /// <summary>
    /// Handler to add setup fees to order
    /// </summary>
    public class SetupFeesHandler : OrderDataHandler
    {
        /// <summary>
        /// Handle items with category "SeupFee"
        /// </summary>
        public virtual IEnumerable<string> HandledCategories
        {
            get { return new[] { "SetupFee" }; }
        }

        /// <summary>
        /// Add any setup fees to order
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var setupFeeItems = orderContext.ItemData.Where(i => this.HandledCategories.Intersect(i.Categories.Select(c => c.Name)).Count() > 0);

            foreach (var item in setupFeeItems)
            {
                Add(order, new PublicOrderItem
                {
                    ItemId = Guid.Empty,
                    ItemNumber = item.ArticleNumber,
                    RenewalPeriodId = item.RenewalPeriodId,
                    Quantity = 1
                });
            }

            return order;
        }
    }
}
