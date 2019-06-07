using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Store.ExistingCustomer.Adapters;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.ShopNameProvider;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Atomia.Store.Themes.Default
{
    public class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            RegisterCoreAdapters(container);

            RegisterProductListingProviders(container);

            RegisterDomainsProvider(container);

            RegisterViewModels(container);

            RegisterPaymentMethods(container);

            RegisterOrderHandling(container);

            RegisterExistingCustomerOrdering(container);

            // Un-comment to use fake static data instead of public order api.
            // RegisterFakeAdapters(container);
        }

        /// <summary>
        /// Register core types for using Asp.Net MVC and Atomia Billing public order api.
        /// </summary>
        private static void RegisterCoreAdapters(UnityContainer container)
        {
            container.RegisterType<IModelBinderProvider, Atomia.Store.AspNetMvc.Infrastructure.ModelBinderProvider>();
            container.RegisterType<IAuditLogger, Atomia.Store.ActionTrail.Adapters.AuditLogger>();
            container.RegisterType<ILogger, Atomia.Store.ActionTrail.Adapters.Logger>();
            container.RegisterType<ICartProvider, Atomia.Store.AspNetMvc.Adapters.CartProvider>();
            container.RegisterType<IContactDataProvider, Atomia.Store.AspNetMvc.Adapters.ContactDataProvider>();
            container.RegisterType<ICurrencyPreferenceProvider, Atomia.Store.AspNetMvc.Adapters.CurrencyPreferenceProvider>();
            container.RegisterType<ICurrencyFormatter, Atomia.Store.AspNetMvc.Adapters.CurrencyFormatter>();
            container.RegisterType<IResourceProvider, Atomia.Store.WebBase.Adapters.ResourceProvider>();
            container.RegisterType<ICustomerTypeProvider, Atomia.Store.AspNetMvc.Adapters.CustomerTypeProvider>();
            container.RegisterType<PaymentUrlProvider, Atomia.Store.AspNetMvc.Adapters.PaymentUrlProvider>();
            container.RegisterType<IThemeNamesProvider, Atomia.Store.AspNetMvc.Adapters.ThemeNamesProvider>();
            container.RegisterType<ICountryProvider, Atomia.Store.PublicBillingApi.Adapters.CountryProvider>("apiProvider");
            container.RegisterType<ICountryProvider, Atomia.Store.PublicBillingApi.Adapters.CachedCountryProvider>(
                new InjectionConstructor(new ResolvedParameter<ICountryProvider>("apiProvider")));
            container.RegisterType<IResellerProvider, Atomia.Store.PublicBillingApi.Adapters.ResellerProvider>();
            container.RegisterType<ILanguageProvider, Atomia.Store.PublicBillingApi.Adapters.LanguageProvider>();
            container.RegisterType<IPackageProvider, Atomia.Store.PublicBillingApi.Adapters.PackageProvider>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.PublicBillingApi.Adapters.CurrencyProvider>();
            container.RegisterType<IProductProvider, Atomia.Store.PublicBillingApi.Adapters.ProductProvider>();
            container.RegisterType<IPaymentMethodsProvider, Atomia.Store.PublicBillingApi.Adapters.PaymentMethodsProvider>();
            container.RegisterType<IItemPresenter, Atomia.Store.AspNetMvc.Adapters.ItemPresenter>("defaultPresenter");
            container.RegisterType<IItemPresenter, Atomia.Store.Themes.Default.Adapters.MarkdownItemPresenter>(
                new InjectionConstructor(new ResolvedParameter<IItemPresenter>("defaultPresenter")));
            container.RegisterType<ILanguagePreferenceProvider, Atomia.Store.AspNetMvc.Adapters.LanguagePreferenceProvider>();
            container.RegisterType<IResellerIdentifierProvider, Atomia.Store.AspNetMvc.Adapters.ResellerIdentifierProvider>();
            container.RegisterType<ITermsOfServiceProvider, Atomia.Store.PublicBillingApi.Adapters.TermsOfServiceProvider>();
            container.RegisterType<ICartPricingService, Atomia.Store.PublicBillingApi.Adapters.CartPricingProvider>("apiPricingService");
            container.RegisterType<ICartPricingService, Atomia.Store.Themes.Default.Adapters.DefaultCampaignPricingProvider>("defaultCampaignPricingService",
                new InjectionConstructor(new ResolvedParameter<ICartPricingService>("apiPricingService")));
            container.RegisterType<ICartPricingService, Atomia.Store.PublicBillingApi.Adapters.SetupFeeCartPricingService>(
                new InjectionConstructor(
                    new ResolvedParameter<ICartPricingService>("defaultCampaignPricingService"),
                    new ResolvedParameter<ApiProductsProvider>()));
            container.RegisterType<IOrderFlowValidator, Atomia.Store.Themes.Default.Adapters.OrderFlowValidator>();
            container.RegisterType<IVatDisplayPreferenceProvider, Atomia.Store.PublicBillingApi.Adapters.VatDisplayPreferenceProvider>();
            container.RegisterType<IVatDataProvider, Atomia.Store.AspNetMvc.Adapters.VatDataProvider>();
            container.RegisterType<IVatNumberValidator, Atomia.Store.PublicBillingApi.Adapters.VatNumberValidator>("apiVatNumberValidator");
            container.RegisterType<IVatNumberValidator, Atomia.Store.PublicBillingApi.Adapters.CachedVatNumberValidator>(
                new InjectionConstructor(new ResolvedParameter<IVatNumberValidator>("apiVatNumberValidator"), new ResolvedParameter<IVatDataProvider>()));

            // Public billing api helpers
            container.RegisterType<PublicBillingApiClient, PublicBillingApiClient>();
            container.RegisterType<PublicBillingApiProxy, PublicBillingApiProxy>();
            container.RegisterType<RenewalPeriodProvider, RenewalPeriodProvider>();
            container.RegisterType<IResellerDataProvider, ResellerDataProvider>("apiProvider");
            container.RegisterType<IResellerDataProvider, CachedResellerDataProvider>(
                new InjectionConstructor(
                    new ResolvedParameter<IResellerDataProvider>("apiProvider"),
                    new ResolvedParameter<IResellerIdentifierProvider>()));
            container.RegisterType<ApiProductsProvider, ApiProductsProvider>();
            container.RegisterType<ProductMapper, ProductMapper>();
            container.RegisterType<IShopNameProvider, DefaultShopNameProvider>();
        }

        /// <summary>
        /// Register product listing providers.
        /// </summary>
        private static void RegisterProductListingProviders(UnityContainer container)
        {
            container.RegisterType<IProductListProvider, Atomia.Store.PublicBillingApi.Adapters.CategoryProductsProvider>("Category");
        }

        /// <summary>
        /// Register basic domains provider and any decorators.
        /// </summary>
        private static void RegisterDomainsProvider(UnityContainer container)
        {
            container.RegisterType<IDomainsProvider, Atomia.Store.PublicBillingApi.Adapters.DomainsProvider>("apiProvider");
            container.RegisterType<IDomainsProvider, Atomia.Store.Themes.Default.Adapters.PremiumDomainsProvider>(
                new InjectionConstructor(new ResolvedParameter<IDomainsProvider>("apiProvider")));
        }

        /// <summary>
        /// Register Asp.NET MVC view models.
        /// </summary>
        private static void RegisterViewModels(UnityContainer container)
        {
            container.RegisterType<DomainsViewModel, DomainsViewModel>();
            container.RegisterType<ProductListingViewModel, ProductListingViewModel>();
            container.RegisterType<ProductListingModel, ProductListingModel>();
            container.RegisterType<AccountViewModel, DefaultAccountViewModel>();
            container.RegisterType<CheckoutViewModel, DefaultCheckoutViewModel>();
        }

        /// <summary>
        /// Register payment plugins, forms and handlers
        /// </summary>
        private static void RegisterPaymentMethods(UnityContainer container)
        {
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.AdyenHpp.AdyenHppGuiPlugin>("AdyenHpp");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinGuiPlugin>("DibsFlexwin");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.Invoice.PayWithInvoiceGuiPlugin>("PayWithInvoice");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayExRedirect.PayExRedirectGuiPlugin>("PayExRedirect");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.PayPal.PayPalGuiPlugin>("PayPal");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPay.WorldPayGuiPlugin>("WorldPay");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.WorldPayXml.WorldPayXmlGuiPlugin>("WorldPayXml");
            container.RegisterType<PaymentMethodGuiPlugin, Atomia.Store.Payment.CCPayment.CCPaymentGuiPlugin>("CCPayment");

            container.RegisterType<PaymentMethodForm, Atomia.Store.Payment.Invoice.PayWithInvoiceForm>("PayWithInvoice");
            container.RegisterType<PaymentMethodForm, Atomia.Store.Payment.CCPayment.CCPaymentForm>("CCPayment");

            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.AdyenHpp.AdyenHppHandler>("AdyenHpp");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.DibsFlexwin.DibsFlexwinHandler>("DibsFlexwin");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.Invoice.PayWithInvoiceHandler>("PayWithInvoice");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.PayExRedirect.PayExRedirectHandler>("PayExRedirect");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.PayPal.PayPalHandler>("PayPal");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.WorldPay.WorldPayHandler>("WorldPay");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.WorldPayXml.WorldPayXmlHandler>("WorldPayXml");
            container.RegisterType<PaymentDataHandler, Atomia.Store.Payment.CCPayment.CCPaymentHandler>("CCPayment");
        }

        /// <summary>
        /// Register order placement serivce and related handlers.
        /// </summary>
        private static void RegisterOrderHandling(UnityContainer container)
        {
            // Transaction data handler
            container.RegisterType<TransactionDataHandler, Atomia.Store.PublicOrderHandlers.TransactionDataHandlers.RequestParamsHandler>("RequestParamsHandler");
            container.RegisterType<TransactionDataHandler, Atomia.Store.PublicOrderHandlers.TransactionDataHandlers.PaymentProfileHandler>("PaymentProfilesHandler");

            // Order data handlers and order placement service
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ResellerHandler>("Reseller");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.LanguageHandler>("LanguageHandler");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CurrencyHandler>("Currency");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ContactDataHandlers.MainContactDataHandler>("MainContact");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.ContactDataHandlers.BillingContactDataHandler>("BillingContact");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.OrderAttributesHandler>("OrderAttributes");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CampaignCodeHandler>("CampaignCode");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.VatValidationHandler>("VatValidation");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.IpAddressHandler>("IpAddress");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.RegisterDomainHandler>("RegisterDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.TransferDomainHandler>("TransferDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.OwnDomainHandler>("OwnDomain");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.DefaultHandler>("Default");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.SetupFeesHandler>("SetupFees");
            container.RegisterType<OrderDataHandler, Atomia.Store.PublicOrderHandlers.CartItemHandlers.RemovePostOrderHandler>("RemovePostOrder");

            container.RegisterType<IOrderPlacementService, Atomia.Store.PublicBillingApi.Adapters.OrderPlacementService>();

            // We resolve the parameters manually to control the order the OrderHandlers are applied.
            var orderDataHandlerParams = new ResolvedArrayParameter<OrderDataHandler>(
                new ResolvedParameter<OrderDataHandler>("Reseller"),
                new ResolvedParameter<OrderDataHandler>("LanguageHandler"),
                new ResolvedParameter<OrderDataHandler>("Currency"),
                new ResolvedParameter<OrderDataHandler>("MainContact"),
                new ResolvedParameter<OrderDataHandler>("BillingContact"),
                new ResolvedParameter<OrderDataHandler>("OrderAttributes"),
                new ResolvedParameter<OrderDataHandler>("CampaignCode"),
                new ResolvedParameter<OrderDataHandler>("VatValidation"),
                new ResolvedParameter<OrderDataHandler>("IpAddress"),
                new ResolvedParameter<OrderDataHandler>("RegisterDomain"),
                new ResolvedParameter<OrderDataHandler>("TransferDomain"),
                new ResolvedParameter<OrderDataHandler>("OwnDomain"),
                new ResolvedParameter<OrderDataHandler>("SetupFees"),

                // This is a good position for TLD specific handlers.

                // Default should be placed after all other handlers that add items form the cart to the order, or there is risk of adding the same item twice.
                new ResolvedParameter<OrderDataHandler>("Default"),

                // This is a good position for handlers that add extra items depending on other items in cart, e.g. like HST-APPY in old order page.

                // RemovePostOrder should be placed last to make sure any added postal fees are removed, since they will be added by Atomia Billing.
                new ResolvedParameter<OrderDataHandler>("RemovePostOrder")
            );

            bool loginAfterOrder;
            var loginAfterOrderSetting = ConfigurationManager.AppSettings["LoginAfterOrder"] as String;

            if (!Boolean.TryParse(loginAfterOrderSetting, out loginAfterOrder))
            {
                throw new ConfigurationErrorsException("Could not parse boolean from 'LoginAfterOrder' setting or it is missing.");
            }

            if (loginAfterOrder)
            {
                container.RegisterType<OrderCreator, Atomia.Store.PublicBillingApi.TokenLoginOrderCreator>(
                    new InjectionConstructor(new ResolvedParameter<PaymentUrlProvider>(), orderDataHandlerParams, new ResolvedParameter<PublicBillingApiProxy>()));
            }
            else
            {
                container.RegisterType<OrderCreator, Atomia.Store.PublicBillingApi.SimpleOrderCreator>(
                    new InjectionConstructor(orderDataHandlerParams, new ResolvedParameter<PublicBillingApiProxy>(), new ResolvedParameter<IAuditLogger>()));
            }

            container.RegisterType<PaymentTransactionCreator, Atomia.Store.PublicBillingApi.PaymentTransactionCreator>();

            // These are required since Unity does not handle IEnumerable<T> automatically.
            container.RegisterType<IEnumerable<PaymentDataHandler>, PaymentDataHandler[]>();
            container.RegisterType<IEnumerable<OrderDataHandler>, OrderDataHandler[]>();
            container.RegisterType<IEnumerable<TransactionDataHandler>, TransactionDataHandler[]>();
        }

        /// <summary>
        /// Fake adapters that use static data instead of relying on public order api being available.
        /// </summary>
        private static void RegisterFakeAdapters(UnityContainer container)
        {
            container.RegisterType<ILanguageProvider, Atomia.Store.Fakes.Adapters.FakeLanguageProvider>();
            container.RegisterType<IResellerProvider, Atomia.Store.Fakes.Adapters.FakeResellerProvider>();
            container.RegisterType<ICountryProvider, Atomia.Store.Fakes.Adapters.FakeCountryProvider>();
            container.RegisterType<IProductListProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>("Category");
            container.RegisterType<IProductProvider, Atomia.Store.Fakes.Adapters.FakeCategoryProductsProvider>();
            container.RegisterType<ICartPricingService, Atomia.Store.Fakes.Adapters.FakePricingProvider>();
            container.RegisterType<IPaymentMethodsProvider, Atomia.Store.Fakes.Adapters.FakePaymentMethodsProvider>();
            container.RegisterType<IResellerIdentifierProvider, Atomia.Store.Fakes.Adapters.FakeRootResellerIdentifierProvider>();
            container.RegisterType<IDomainsProvider, Atomia.Store.Fakes.Adapters.FakePremiumDomainsProvider>();
            container.RegisterType<IOrderPlacementService, Atomia.Store.Fakes.Adapters.FakeOrderPlacementService>();
            container.RegisterType<ICurrencyProvider, Atomia.Store.Fakes.Adapters.FakeCurrencyProvider>();
        }

        private static void RegisterExistingCustomerOrdering(UnityContainer container)
        {
            if (!ConfigurationHelper.AllowExistingCustomerOrders())
            {
                return;
            }

            // Existing customer adapters
            container.RegisterType<IContactDataProvider, CombinedContactProvider>();
            container.RegisterType<CustomerLoginValidator, CustomerLoginValidator>();
            container.RegisterType<OrderCreator, CombinedOrderCreator>();
        }
    }
}
