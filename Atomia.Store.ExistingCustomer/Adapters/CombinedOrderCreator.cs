using System.Collections.Generic;
using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.ExistingCustomer.Adapters
{
    public class CombinedOrderCreator : OrderCreator
    {
        private ExistingCustomerContactProvider existingCustomerProvider = new ExistingCustomerContactProvider();
        private ExistingCustomerOrderCreator existingCustomerOrderCreator;
        private SimpleOrderCreator simpleOrderCreator;

        public CombinedOrderCreator(IEnumerable<OrderDataHandler> orderDataHandlers, PublicBillingApiProxy billingApi, IAuditLogger auditLogger = null)
            : base(orderDataHandlers, billingApi, auditLogger)
        {
            this.existingCustomerOrderCreator = new ExistingCustomerOrderCreator(orderDataHandlers, billingApi, auditLogger);
            this.simpleOrderCreator = new SimpleOrderCreator(orderDataHandlers, billingApi, auditLogger);
        }

        public override PublicOrder CreateOrder(PublicOrderContext publicOrderContext, PaymentDataHandler paymentHandler)
        {
            PublicOrder order = null;
            var existingCustomerData = existingCustomerProvider.GetContactData();

            if (existingCustomerData != null)
            {
                order = existingCustomerOrderCreator.CreateOrder(publicOrderContext, paymentHandler);
            }
            else
            {
                order = simpleOrderCreator.CreateOrder(publicOrderContext, paymentHandler);
            }

            return order;
        }
    }
}