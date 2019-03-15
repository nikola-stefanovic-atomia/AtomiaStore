using Atomia.Common.Validation;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Atomia.Web.Plugin.ShopNameProvider;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Contact data model that maps closely to data that can be used with Atomia Billing accounts.
    /// </summary>
    public abstract class ContactModel : ContactData
    {
        private readonly ICustomerTypeProvider customerTypeProvider = DependencyResolver.Current.GetService<ICustomerTypeProvider>();
        private readonly ICountryProvider countryProvider = DependencyResolver.Current.GetService<ICountryProvider>();

        private IndividualExtraInfo individualInfo;
        private CompanyExtraInfo companyInfo;
        private CustomFieldsExtraInfo customFieldsInfo;

        /// <summary>
        /// Construct new instances with instantiated sub-forms.
        /// </summary>
        public ContactModel()
        {
            var resellerProvider = DependencyResolver.Current.GetService<IResellerProvider>();
            var cartProvider = DependencyResolver.Current.GetService<ICartProvider>();

            var resellerId = resellerProvider.GetReseller().Id;
            var cart = cartProvider.GetCart();

            this.ResellerId = resellerId;
            this.CartItems = cart.CartItems.ToList();
        }

        /// <summary>
        /// Partial view name based on the <see cref="ContactData"/> id.
        /// </summary>
        public virtual string PartialViewName
        {
            get
            {
                return "_" + this.Id;
            }
        }

        /// <summary>
        /// Available <see cref="Atomia.Store.Core.CustomerType">CustomerTypes</see> usable as select list.
        /// </summary>
        public virtual IEnumerable<SelectListItem> CustomerTypeOptions {
            get
            {
                var customerTypes = customerTypeProvider.GetCustomerTypes();

                return customerTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                });
            }
        }

        /// <summary>
        /// Available <see cref="Atomia.Store.Core.Country">Countires</see> usable as select list.
        /// </summary>
        public virtual IEnumerable<SelectListItem> CountryOptions
        {
            get
            {
                var countries = countryProvider.GetCountries();
                var defaultCountry = countryProvider.GetDefaultCountry();

                return countries.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code,
                    Selected = (x.Code == defaultCountry.Code)
                }).OrderBy(x => x.Text);
            }
        }

        /// <summary>
        /// Customer selected customer type.
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public virtual string CustomerType { get; set; }

        /// <summary>
        /// Customer provided email address
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Email, "CustomerValidation,Email")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Customer provided first name
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.FirstName, "CustomerValidation,FirstName")]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Customer provided last name
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.LastName, "CustomerValidation,LastName")]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Customer provided address
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Address, "CustomerValidation,Address")]
        public virtual string Address { get; set; }

        /// <summary>
        /// Customer provided second address line
        /// </summary>
        [CustomerValidation(CustomerValidationType.Address, "CustomerValidation,address")]
        public virtual string Address2 { get; set; }

        /// <summary>
        /// Customer provided city
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.City, "CustomerValidation,City")]
        public virtual string City { get; set; }

        /// <summary>
        /// Customer provided zip code
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Zip, "CustomerValidation,Zip", CountryField = "Country")]
        public virtual string Zip { get; set; }

        /// <summary>
        /// Customer selected country
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Country, "CustomerValidation,Country", CountryField = "Country", ProductField = "CartItems.ArticleNumber" , ResellerIdField = "ResellerId")]
        public override string Country { get; set; }

        /// <summary>
        /// Customer provided state
        /// </summary>
        public virtual string State { get; set; }

        /// <summary>
        /// Customer provided phone number
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Phone, "CustomerValidation,Phone", CountryField = "Country")]
        public virtual string Phone { get; set; }

        /// <summary>
        /// Customer provided fax number
        /// </summary>
        [CustomerValidation(CustomerValidationType.Fax, "CustomerValidation,Fax", CountryField = "Country")]
        public virtual string Fax { get; set; }

        /// <summary>
        /// <see cref="IndividualExtraInfo"/> sub-form.
        /// </summary>
        public virtual IndividualExtraInfo IndividualInfo
        {
            get
            {
                if (individualInfo == null)
                {
                    individualInfo = new IndividualExtraInfo();
                }

                if (individualInfo.Parent == null)
                {
                    individualInfo.Parent = this;
                }

                return individualInfo;
            }

            set { individualInfo = value;  }
        }

        /// <summary>
        /// <see cref="CompanyExtraInfo"/> sub-form.
        /// </summary>
        public virtual CompanyExtraInfo CompanyInfo
        {
            get
            {
                if (companyInfo == null)
                {
                    companyInfo = new CompanyExtraInfo();
                }

                if (companyInfo.Parent == null)
                {
                    companyInfo.Parent = this;
                }

                return companyInfo;
            }

            set { companyInfo = value;  }
        }

        /// <summary>
        /// Reseller id.
        /// </summary>
        /// <remarks>Property is required by some types of CustomerValidation.</remarks>
        public Guid ResellerId { get; set; }

        /// <summary>
        /// List of <see cref="CartItem"/> from current <see cref="Cart"/>.
        /// </summary>
        /// <remarks>Property is required by some types of CustomerValidation.</remarks>
        public List<CartItem> CartItems { get; set; }

        /// <summary>
        /// <see cref="CustomFieldsExtraInfo"/> sub-form.
        /// </summary>
        public virtual CustomFieldsExtraInfo CustomFieldsInfo
        {
            get
            {
                if (customFieldsInfo == null)
                {
                    customFieldsInfo = new CustomFieldsExtraInfo();
                }

                if (customFieldsInfo.Parent == null)
                {
                    customFieldsInfo.Parent = this;
                }

                return customFieldsInfo;
            }

            set { customFieldsInfo = value; }
        }

        /*/// <summary>
        /// Gets or sets the serialized custom fields.
        /// </summary>
        /// <value>The custom fields.</value>
        public string CustomFields { get; set; }

        /// <summary>
        /// Gets or sets the custom fields dictionary.
        /// </summary>
        /// <value>The custom fields dictionary.</value>
        [CustomFieldsValidation("CustomFields", "CustomerValidation,CustomFields", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public IDictionary<string, string> CustomFieldsDict { get; set; }*/
    }

    /// <summary>
    /// Any sub-forms must implement this class to be compatible with CustomerValidation since certain
    /// properties are expected on the form being validated, i.e. the sub-form, but are in this case provided by the customer via the parent form.
    /// </summary>
    /// <remarks>The Parent property should be set by the parent form</remarks>
    public class ContactSubmodel
    {
        /// <summary>
        /// Reference to the parent <see cref="ContactModel"/>
        /// </summary>
        internal ContactModel Parent { get; set; }

        /// <summary>
        /// Proxy to the Country property on the parent form.
        /// </summary>
        public string Country
        {
            get
            {
                return Parent.Country;
            }
        }

        /// <summary>
        /// Proxy to the CartItems property on the parent form.
        /// </summary>
        public List<CartItem> CartItems
        {
            get
            {
                return Parent.CartItems;
            }
        }

        /// <summary>
        /// Proxy to the ResellerId property on the parent form.
        /// </summary>
        public Guid ResellerId
        {
            get
            {
                return Parent.ResellerId;
            }
        }
    }

    /// <summary>
    /// A <see cref="ContactSubmodel"/> form that collects data only relevant for company customers.
    /// </summary>
    public class CompanyExtraInfo : ContactSubmodel
    {
        /// <summary>
        /// Customer provided company name
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.CompanyName, "CustomerValidation,CompanyName")]
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// Customer provided national corporate identification / registration number for the company
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,CompanyIdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string IdentityNumber { get; set; }

        /// <summary>
        /// Customer provided VAT registration number for the company
        /// </summary>
        [CustomerValidation(CustomerValidationType.VatNumber, "CustomerValidation,VatNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string VatNumber { get; set; }

    }

    /// <summary>
    /// A <see cref="ContactSubmodel"/> form that collects data only relevant for individual (private person) customers.
    /// </summary>
    public class IndividualExtraInfo : ContactSubmodel
    {
        /// <summary>
        /// Customer provided national identification / identity / registration number for the customer
        /// </summary>
        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,IndividualIdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string IdentityNumber { get; set; }
    }

    /// <summary>
    /// A <see cref="ContactSubmodel"/> form that collects data only for custom fields.
    /// </summary>
    public class CustomFieldsExtraInfo : ContactSubmodel
    {
        public CustomFieldsExtraInfo()
        {
            CustomFieldsDict = new Dictionary<string, string>();
        }
        /// <summary>
        /// Gets or sets the serialized custom fields.
        /// </summary>
        /// <value>The custom fields.</value>
        public string CustomFields { get; set; }

        /// <summary>
        /// Gets or sets the custom fields dictionary.
        /// </summary>
        /// <value>The custom fields dictionary.</value>
        [CustomFieldsValidation("CustomFields", "CustomerValidation,CustomFields", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public IDictionary<string, string> CustomFieldsDict { get; set; }
    }
}
