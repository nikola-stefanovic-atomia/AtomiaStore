using Atomia.Store.Core;
using Atomia.Store.ExistingCustomer.Adapters;
using Atomia.Store.ExistingCustomer.Models;
using System.Web.Mvc;

namespace Atomia.Store.ExistingCustomer.Controllers
{

    /// <summary>
    /// Existing customer validation, part of order flow
    /// </summary>
    public sealed class ExistingCustomerController : Controller
    {
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();
        private readonly CustomerLoginValidator customerLoginValidator = DependencyResolver.Current.GetService<CustomerLoginValidator>();

        /// <summary>
        /// Existing customer validation handler. Redirects to checkout.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateLogin(CustomerLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var contactData = customerLoginValidator.ValidateCustomerLogin(model.Username, model.Password);

                if (contactData.Valid)
                {
                    var existingCustomer = new ExistingCustomerContactModel();
                    existingCustomer.SetContactData(contactData);

                    contactDataProvider.SaveContactData(existingCustomer);

                    return RedirectToAction("Index", "Checkout");
                }
                else
                {
                    TempData["ExistingCustomerValidateLoginFailed"] = resourceProvider.GetResource("InvalidUsernameOrPassword");
                }
            }

            return RedirectToAction("Index", "Account");
        }
    }
}
