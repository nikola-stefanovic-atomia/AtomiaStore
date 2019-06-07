using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Viem model which represent the type of account that customers can use when placing new order.
    /// <para> 1. Customers can use only new accounts. </para>
    /// <para> 2. Customers can use exiting or new accounts. </para> 
    /// </summary>
    public class CustomerOrderAccountSelectorModel
    {
        /// <summary>
        /// The package provider instance
        /// </summary>
        private IPackageProvider packageProvider = DependencyResolver.Current.GetService<IPackageProvider>();

        /// <summary>
        /// New account type id for customers order
        /// </summary>
        public CustomerOrderAccounType NewAccountType { get; set; }

        /// <summary>
        /// Exiting account type id for customers order
        /// </summary>
        public CustomerOrderAccounType ExistingAccountType { get; set; }

        /// <summary>
        /// Indicates whether customers can place orders with exiting accounts
        /// </summary>
        public bool AllowExistingCustomerOrders
        {
            get
            {
                return ConfigurationHelper.AllowExistingCustomerOrders()
                    && packageProvider.IsMultiPackageEnabled();
            }
        }


        public CustomerOrderAccountSelectorModel()
        {
            NewAccountType = CustomerOrderAccounType.New;
            ExistingAccountType = CustomerOrderAccounType.Existing;
        }
    }

    /// <summary>
    /// The types of account for customer to purchase a new order
    /// </summary>
    public enum CustomerOrderAccounType
    {
        /// <summary>
        /// Exiting customer account
        /// </summary>
        Existing,

        /// <summary>
        /// New customer account
        /// </summary>
        New
    }
}
