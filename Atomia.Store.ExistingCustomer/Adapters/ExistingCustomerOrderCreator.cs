using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atomia.ActionTrail.Base;
using Atomia.Store.Core;
using Atomia.Store.ExistingCustomer.Models;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.PublicOrderHandlers.ContactDataHandlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.ExistingCustomer.Adapters
{
    public class ExistingCustomerOrderCreator : OrderCreator
    {
        private AtomiaBillingPublicService service = DependencyResolver.Current.GetService<AtomiaBillingPublicService>();
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly IEnumerable<OrderDataHandler> noContactOrderDataHandlers;

        public ExistingCustomerOrderCreator(IEnumerable<OrderDataHandler> orderDataHandlers, PublicBillingApiProxy billingApi, IAuditLogger auditLogger = null) 
            : base(orderDataHandlers, billingApi, auditLogger)
        {
            this.noContactOrderDataHandlers = orderDataHandlers.Where(o => o.GetType() != typeof(MainContactDataHandler) && o.GetType() != typeof(BillingContactDataHandler));
        }

        protected PublicOrder PrepareOrderExistingCustomer(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            var newOrder = new PublicOrder()
            {
                OrderItems = new PublicOrderItem[0],
                CustomData = new PublicOrderCustomData[0]
            };

            foreach (var handler in this.noContactOrderDataHandlers)
            {
                newOrder = handler.AmendOrder(newOrder, publicOrderContext);

                if (newOrder == null)
                {
                    throw new InvalidOperationException("OrderDataHandler must return a non-null order from AmendOrder.");
                }
            }

            newOrder.PaymentMethod = paymentHandler.PaymentMethodType;

            // Only run the selected payment handler.
            newOrder = paymentHandler.AmendOrder(newOrder, publicOrderContext.PaymentData);

            if (newOrder == null)
            {
                throw new InvalidOperationException("PaymentDataHandler must return a non-null order from AmendOrder.");
            }

            return newOrder;
        }

        /// <summary>
        /// Create PublicOrder and call CreateOrder in Atomia Billing Public Service.
        /// </summary>
        /// <param name="publicOrderContext">Order data</param>
        /// <param name="paymentHandler">Handler for customer's selected payment method</param>
        /// <returns>The order object returned from Atomia Billing Public Service</returns>
        public override PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            var newOrder = PrepareOrderExistingCustomer(publicOrderContext, paymentHandler);
            var existingCustomerData = contactDataProvider.GetContactData();

            if (existingCustomerData != null)
            {
                var contactData = existingCustomerData.GetContactData();
                var customerData = (ExistingCustomerContactData)contactData.First();
                var createdOrder = service.CreateOrderExistingCustomer(newOrder, customerData.Username, customerData.Password, customerData.CustomerNumber);

                if (createdOrder == null)
                {
                    throw new InvalidOperationException("Order for existing customer could not be created.");
                }

                if (this.auditLogger != null)
                {
                    this.auditLogger.Log(AuditActionTypes.OrderCreated, string.Empty, createdOrder.CustomerId.ToString(), customerData.Username, createdOrder.Id.ToString(), null);
                }

                return createdOrder;
            }

            return null;
        }
    }
}
