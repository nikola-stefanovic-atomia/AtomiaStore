using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Abstract view model for the /Account/Index page
    /// </summary>
    public abstract class AccountViewModel : IContactDataCollection
    {
        /// <summary>
        /// Get <see cref="Atomia.Store.Core.ContactData"/> with collected information from customer.
        /// </summary>
        public abstract IEnumerable<ContactData> GetContactData();

        public abstract void SetContactData(IContactDataCollection previousContactData);
    }


    /// <summary>
    /// The default implementation of <see cref="AccountViewModel"/>. 
    /// Collects a main contact and optionally separate billing and WHOIS contacts
    /// </summary>
    public class DefaultAccountViewModel : AccountViewModel
    {
        /// <summary>
        /// Constructor intializing the sub-forms
        /// </summary>
        public DefaultAccountViewModel()
        {
            MainContact = new MainContactModel();
            BillingContact = new BillingContactModel();
            WhoisContact = new WhoisContactModel();
            NoridDeclaration = new NoridDeclarationModel();
            OrderAccountTypeSelector = new CustomerOrderAccountSelectorModel();
        }

        /// <summary>
        /// Main contact data collected from the customer.
        /// </summary>
        public MainContactModel MainContact { get; set; }

        /// <summary>
        /// Billing contact data collected from the customer.
        /// </summary>
        public BillingContactModel BillingContact { get; set; }

        /// <summary>
        /// WHOIS contact data collected from the customer.
        /// </summary>
        public WhoisContactModel WhoisContact { get; set; }

        /// <summary>
        /// Data for NORID declaration when .no domain is added to cart.
        /// </summary>
        public NoridDeclarationModel NoridDeclaration { get; set; }

        /// <summary>
        /// Selects the new or exiting account when customers create a new order
        /// </summary>
        public CustomerOrderAccountSelectorModel OrderAccountTypeSelector { get; set; }

        /// <summary>
        /// If other billing contact than main contact should be used or not.
        /// </summary>
        public bool OtherBillingContact { get; set; }

        /// <summary>
        /// If other WHOIS contact than main contact should be used or not.
        /// </summary>
        public bool OtherWhoisContact { get; set; }

        /// <summary>
        /// Get the collected contact data.
        /// </summary>
        public override IEnumerable<ContactData> GetContactData()
        {
            return new ContactData[] {
                MainContact,
                BillingContact,
                WhoisContact
            };
        }

        public override void SetContactData(IContactDataCollection previousContactData)
        {
            var allContacts = previousContactData.GetContactData();
            var main = allContacts.FirstOrDefault(c => c.Id == "MainContact") as MainContactModel;
            var billing = allContacts.FirstOrDefault(c => c.Id == "BillingContact") as BillingContactModel;
            var whois = allContacts.FirstOrDefault(c => c.Id == "WhoisContact") as WhoisContactModel;

            // Email is required so if it is present the form has been filled in.

            if (main != null && !string.IsNullOrEmpty(main.Email))
            {
                this.MainContact = main;
            }

            if (billing != null && !string.IsNullOrEmpty(billing.Email))
            {
                this.BillingContact = billing;
                this.OtherBillingContact = true;
            }

            if (whois != null && !string.IsNullOrEmpty(whois.Email))
            {
                this.WhoisContact = whois;
                this.OtherWhoisContact = true;
            }
        }
    }
}
