using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.PublicBillingApi.Handlers
{
    /// <summary>
    /// Base for adding order data to an Atomia Billing Public Service Order.
    /// </summary>
    public abstract class OrderDataHandler
    {
        /// <summary>
        /// Helper for normalizing field string data before adding to order. Converts various "null" strings to empty string, trims whitespace.
        /// </summary>
        /// <param name="orderDataField">The string to normalize</param>
        /// <returns>The normalized string.</returns>
        public static string NormalizeData(string orderDataField)
        {
            if (String.IsNullOrEmpty(orderDataField))
            {
                return String.Empty;
            }

            if (String.Compare(orderDataField, "null") == 0 || String.Compare(orderDataField, "NULL") == 0 || String.Compare(orderDataField, "Null") == 0)
            {
                return String.Empty;
            }

            return orderDataField.Trim();
        }

        /// <summary>
        /// Amend order with data from context
        /// </summary>
        /// <param name="order">The order to amend</param>
        /// <param name="orderContext">The order data</param>
        /// <returns>The amended order</returns>
        public abstract PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext);

        /// <summary>
        /// Instance wrapper for <see cref="NormalizeData"/>.
        /// </summary>
        protected string Normalize(string orderDataField)
        {
            return NormalizeData(orderDataField);
        }

        /// <summary>
        /// Add item to order.
        /// </summary>
        /// <param name="order">The order to add item to</param>
        /// <param name="item">The item to add</param>
        protected void Add(PublicOrder order, PublicOrderItem item)
        {
            var orderItems = new List<PublicOrderItem>(order.OrderItems);

            orderItems.Add(item);

            order.OrderItems = orderItems.ToArray();
        }


        /// <summary>
        /// Add custom attribute to order.
        /// </summary>
        /// <param name="order">The order to add custom attribute to</param>
        /// <param name="item">The custom attribute to add</param>
        protected void Add(PublicOrder order, PublicOrderCustomData customData)
        {
            var customAttributes = new List<PublicOrderCustomData>(order.CustomData);

            customAttributes.Add(customData);

            order.CustomData = customAttributes.ToArray();
        }

        /// <summary>
        /// Get matching item if it exists in order already.
        /// Must match all of the following: article number, renewal period and "DomainName" custom attributes if it exists.
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <param name="item">The item to check for match in order</param>
        /// <returns>The matching order item or null</returns>
        protected PublicOrderItem ExistingOrderItem(PublicOrder order, ItemData item)
        {
            PublicOrderItem existingOrderItem = null;
            var domainAttr = item.CartItem.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");

            // do not use "DomainName" custom attribute on hosting items for identification
            var hostingCategories = new[] { "HostingPackage", "DNS" };
            bool isHostingItem = hostingCategories.Intersect(item.Categories.Select(c => c.Name)).Count() > 0;

            if (domainAttr != null && !isHostingItem)
            {
                existingOrderItem = order.OrderItems.FirstOrDefault(orderItem =>
                    orderItem.ItemNumber == item.ArticleNumber &&
                    orderItem.RenewalPeriodId == item.RenewalPeriodId &&
                    orderItem.CustomData.Any(x => x.Name == "DomainName" && x.Value == domainAttr.Value));
            }
            else
            {
                existingOrderItem = order.OrderItems.FirstOrDefault(orderItem =>
                    orderItem.ItemNumber == item.ArticleNumber &&
                    orderItem.RenewalPeriodId == item.RenewalPeriodId);
            }

            return existingOrderItem;
        }

        /// <summary>
        /// Check if a matching item exists in order already.
        /// </summary>
        /// <param name="order">The order to check</param>
        /// <param name="item">The item to match</param>
        /// <returns>true if item matches, false otherwise</returns>
        protected bool OrderItemsContain(PublicOrder order, ItemData item)
        {
            return ExistingOrderItem(order, item) != null;
        }
    }
}
