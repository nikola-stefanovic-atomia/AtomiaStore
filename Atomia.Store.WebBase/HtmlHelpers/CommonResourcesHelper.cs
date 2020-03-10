using System;
using System.Web;
using System.Web.Mvc;
using Atomia.Store.Core;

namespace Atomia.Store.WebBase.HtmlHelpers
{
    /// <summary>
    /// HTML helper extensions that get resource strings only from Common.resx in App_GlobalResources
    /// using  localization extensions from Atomia.Web.Base
    /// </summary>
    public static class CommonResourcesHelper
    {
        private static IThemeNamesProvider themeNamesProvider = DependencyResolver.Current.GetService<IThemeNamesProvider>();

        /// <summary>
        /// Get escaped resource string from App_GlobalResources/Common.resx
        /// </summary>
        public static string CommonResource(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetCurrentThemeName() + "Common,";
            var resource = LocalizationHelpers.Resource(htmlhelper, themeClass + expression, args);

            return String.IsNullOrEmpty(resource)
                ? LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args)
                : resource;
        }

        /// <summary>
        /// Gets escaped resource string from App_GlobalResources/Common.resx or from theme common folder.
        /// </summary>
        public static string GlobalCommonResource(this Controller controller, string expression, params object[] args)
        {
            var theme = themeNamesProvider.GetCurrentThemeName();
            var themeClass = theme + "Common,";
            var resource = LocalizationHelpers.GlobalResource(themeClass + expression, theme);

            return string.IsNullOrWhiteSpace(resource)
                ? LocalizationHelpers.Resource(controller, "Common," + expression, args)
                : resource;
        }

        /// <summary>
        /// Get un-escaped resource string from App_GlobalResources/Common.resx
        /// </summary>
        public static IHtmlString CommonResourceRaw(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetCurrentThemeName() + "Common,";
            var resource = LocalizationHelpers.Resource(htmlhelper, themeClass + expression, args);

            if (String.IsNullOrEmpty(resource))
            {
                resource = LocalizationHelpers.Resource(htmlhelper, "Common," + expression, args);
            }

            return htmlhelper.Raw(resource);
        }

        /// <summary>
        /// Get JavaScript escaped resource string from App_GlobalResources/Common.resx
        /// </summary>
        public static string CommonResourceJs(this HtmlHelper htmlhelper, string expression, params object[] args)
        {
            var themeClass = themeNamesProvider.GetCurrentThemeName() + "Common,";
            var resource = LocalizationHelpers.ResourceJavascript(htmlhelper, themeClass + expression, args);

            return String.IsNullOrEmpty(resource)
                ? LocalizationHelpers.ResourceJavascript(htmlhelper, "Common," + expression, args)
                : resource;
        }
    }
}
