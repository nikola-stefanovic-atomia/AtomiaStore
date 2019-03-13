using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Atomia.Store.Payment.CCPayment
{
    /// <summary>
    /// ValidationAttribute that wraps standard creditcard type but let's us set error message from IResourceProvider.
    /// </summary>
    public sealed class CCPaymentCreditCardAttribute : DataTypeAttribute, IClientValidatable
    {
        public CCPaymentCreditCardAttribute(DataType dataType) : base(dataType)
        {
            var resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();
            this.ErrorMessage = resourceProvider.GetResource("ErrorCreditCard");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "creditcard",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
        }
    }

    /// <summary>
    /// Form for collecting credit card information.
    /// </summary>
    public sealed class CCPaymentForm : PaymentMethodForm
    {
        private readonly List<SelectListItem> monthOptions;
        private readonly List<SelectListItem> yearOptions;

        public CCPaymentForm()
        {
            this.monthOptions = new List<SelectListItem>
            {
                new SelectListItem() { Text = "01", Value = "1" },
                new SelectListItem() { Text = "02", Value = "2" },
                new SelectListItem() { Text = "03", Value = "3" },
                new SelectListItem() { Text = "04", Value = "4" },
                new SelectListItem() { Text = "05", Value = "5" },
                new SelectListItem() { Text = "06", Value = "6" },
                new SelectListItem() { Text = "07", Value = "7" },
                new SelectListItem() { Text = "08", Value = "8" },
                new SelectListItem() { Text = "09", Value = "9" },
                new SelectListItem() { Text = "10", Value = "10" },
                new SelectListItem() { Text = "11", Value = "11" },
                new SelectListItem() { Text = "12", Value = "12" }
            };

            var currentYear = DateTime.Now.Year;
            var yearOptions = new List<SelectListItem>();

            for(var i = 0; i < 15; i += 1)
            {
                string year = string.Format("{0}", currentYear + i);
                yearOptions.Add(new SelectListItem() { Text = year, Value = year });
            }

            this.yearOptions = yearOptions;
        }

        public override string Id
        {
            get { return "CCPayment"; }
        }

        /// <summary>
        /// The credit card number
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CCPaymentCreditCard(DataType.CreditCard)]
        public string CardNumber { get; set; }

        /// <summary>
        /// The security code, aka CSC/CVV/CVC/CID.
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [AtomiaRegularExpression("^$|^[0-9]{3,4}$", "Common,ErrorInvalidCardSecurityCode", false)]
        public string CardSecurityCode { get; set; }

        /// <summary>
        /// Month part of card expiration date.
        /// </summary>
        [AtomiaRequired("Common,ErrorExpiresMonthRequired")]
        public int? ExpiresMonth { get; set; }

        /// <summary>
        /// Year part of card expiration date.
        /// </summary>
        [AtomiaRequired("Common,ErrorExpiresYearRequired")]
        public int? ExpiresYear { get; set; }

        /// <summary>
        /// Card owner
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string CardOwner { get; set; }

        /// <summary>
        /// Options for expires month dropdown
        /// </summary>
        public IEnumerable<SelectListItem> MonthOptions { get { return monthOptions; } }

        /// <summary>
        /// Options for expires year dropdown
        /// </summary>
        public IEnumerable<SelectListItem> YearOptions { get { return yearOptions; } }
    }
}
