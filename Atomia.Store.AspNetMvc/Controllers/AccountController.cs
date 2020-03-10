﻿using Atomia.Store.AspNetMvc.Filters;
using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.Core;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Account data collection, part of order flow
    /// </summary>
    public sealed class AccountController : Controller
    {
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();

        /// <summary>
        /// Account form page.
        /// </summary>
        [OrderFlowFilter]
        [HttpGet]
        public ActionResult Index()
        {
            var model = DependencyResolver.Current.GetService<AccountViewModel>();
            var previousContactData = contactDataProvider.GetContactData();

            if (previousContactData != null)
            {
                model.SetContactData(previousContactData);
            }

            var orderFlow = (OrderFlowModel)ViewBag.OrderFlow;
            if (orderFlow.Steps.Count() == 1)
            {
                // first step, mark that DNS package should be added.
                ViewBag.AddDnsPackage = ConfigurationHelper.GetDnsPackageArticleNumber();
            }

            if (TempData.ContainsKey("ExistingCustomerValidateLoginFailed"))
            {
                ModelState.AddModelError("CustomerLogin", TempData["ExistingCustomerValidateLoginFailed"].ToString());
            }

            SetSelectedCustomerType(previousContactData);

            return View(model);
        }

        /// <summary>
        /// Account form handler. Redirects to checkout.
        /// </summary>
        [OrderFlowFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                contactDataProvider.SaveContactData(model);
                var orderFlow = (OrderFlowModel)ViewBag.OrderFlow;
                var routeValues = orderFlow.IsQueryStringBased ? new { flow = orderFlow.Name } : null;

                if (orderFlow.Steps.Count() == orderFlow.CurrentStep.StepNumber)
                {
                    // last step, do the checkout.
                    return RedirectToAction("CheckoutAccount", "Checkout", routeValues);
                }

                return RedirectToAction("Index", "Checkout", routeValues);
            }

            ViewBag.SelectedOrderAccounType = CustomerOrderAccounType.New;

            return View(model);
        }

        /// <summary>
        /// Determines which kind of contact is selected.
        /// </summary>
        /// <param name="previousContactData">Contact data collection</param>
        private void SetSelectedCustomerType(IContactDataCollection previousContactData)
        {
            ViewBag.SelectedOrderAccounType = CustomerOrderAccounType.New;
            if (previousContactData != null && previousContactData.GetType() == typeof(ExistingCustomerContactModel))
            {
                ViewBag.SelectedOrderAccounType = CustomerOrderAccounType.Existing;
            }
        }

        /// <summary>
        /// Norid terms of service page.
        /// </summary>
        [HttpGet]
        public ActionResult NoridTermsOfService(string domains, string domainsIDN, string applicantName, string applicantNumber, string name, string time, string customerType)
        {
            ViewBag.domains = domains ?? "";
            ViewBag.domainsIDN = domainsIDN ?? "";
            ViewBag.applicantName = applicantName ?? "";
            ViewBag.applicantNumber = applicantNumber ?? "";
            ViewBag.name = name ?? "";
            ViewBag.time = time ?? "";
            ViewBag.isCompany = customerType == "company";

            return View();
        }
    }
}
