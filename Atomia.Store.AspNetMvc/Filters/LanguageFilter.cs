using Atomia.Store.Core;
using System.Threading;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Filters
{
    /// <summary>
    /// Filter for setting language based on query string "?lang={languageTag}".
    /// Sets preference via <see cref="Atomia.Store.Core.ILanguagePreferenceProvider"/>, and sets thread CurrentCulture and CurrentUICulture.
    /// </summary>
    public sealed class LanguageFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            
            if (filterContext.Controller.ControllerContext.IsChildAction)
            {
                return;
            }

            var languagePreferenceProvider = DependencyResolver.Current.GetService<ILanguagePreferenceProvider>();

            if (request.QueryString["lang"] != null)
            {
                var resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

                var languageTag = request.QueryString["lang"];
                var language = Language.CreateLanguage(resourceProvider, languageTag);

                languagePreferenceProvider.SetPreferredLanguage(language);
            }

            var currentLanguage = languagePreferenceProvider.GetCurrentLanguage();
            var currentCulture = currentLanguage.AsCultureInfo();

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = currentCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
        }
    }
}
