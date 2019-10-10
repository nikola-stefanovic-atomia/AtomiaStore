using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class LabelViewModel
    {
        /// <summary>
        /// The package provider instance
        /// </summary>
        private IPackageProvider packageProvider = DependencyResolver.Current.GetService<IPackageProvider>();

        /// <summary>
        /// Indicates whether customers can set a label for the package
        /// </summary>
        public virtual bool IsPackageLabelEnabled
        {
            get
            {
                return ConfigurationHelper.IsPackageLabelEnabled()
                    && packageProvider.IsMultiPackageEnabled();
            }
        }

        /// <summary>
        /// The label value.
        /// </summary>
        [AtomiaRegularExpression("SubscriptionLabel", "Common,ErrorInvalidLabel", true)]
        public virtual string Value { get; set; }

    }
}
