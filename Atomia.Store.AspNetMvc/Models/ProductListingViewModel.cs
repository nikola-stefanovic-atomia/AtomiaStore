
using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for the /ProductListing/Index page.
    /// </summary>
    public class ProductListingViewModel
    {
        /// <summary>
        /// The package provider instance
        /// </summary>
        private IPackageProvider packageProvider = DependencyResolver.Current.GetService<IPackageProvider>();

        /// <summary>
        /// Query to use for listing products
        /// </summary>
        public virtual string Query { get; set; }

        /// <summary>
        /// Name of <see cref="Atomia.Store.Core.IProductListProvider" to use. />
        /// </summary>
        public virtual string ListingType { get; set; }

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
        /// Name of <see cref="Atomia.Store.AspNetMvc.Models.LabelViewModel" to use. />
        /// </summary>
        public virtual LabelViewModel Label { get; set; }
    }
}
