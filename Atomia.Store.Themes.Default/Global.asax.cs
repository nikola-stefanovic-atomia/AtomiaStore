using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Configuration;
using Atomia.Store.AspNetMvc.Infrastructure;

namespace Atomia.Store.Themes.Default
{
    /// <summary>
    /// Wrapper application around <see cref="Atomia.Store.AspNetMvc.Infrastructure.GlobalEventsHandler"/>
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Initialize a new instance around <see cref="Atomia.Store.AspNetMvc.Infrastructure.GlobalEventsHandler"/> specified in app settings.
        /// </summary>
        public MvcApplication()
        {
            this.HandlerClass = null;

            var globalEventsHandlerType = ConfigurationManager.AppSettings["GlobalEventsHandler"];

            Type t = Type.GetType(globalEventsHandlerType);
            if (t != null)
            {
                this.HandlerClass = (GlobalEventsHandler)Activator.CreateInstance(t);
            }
            else
            {
                throw new Exception("Could not instantiate GlobalEventsHandler");
            }
        }

        /// <summary>
        /// Gets or sets the handler class.
        /// </summary>
        /// <value>The handler class.</value>
        private GlobalEventsHandler HandlerClass { get; set; }

        /// <summary>
        /// Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
            this.HandlerClass.Session_Start(sender, e);
        }

        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            this.HandlerClass.Application_Start(sender, e);
        }

        /// <summary>
        /// Handles the PreRequestHandlerExecute event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            this.HandlerClass.Application_PreRequestHandlerExecute(sender, e);
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.HandlerClass.Application_BeginRequest(sender, e);
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            this.HandlerClass.Application_Error(sender, e);
        }

        /// <summary>
        /// Handles the Init event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Init(object sender, EventArgs e)
        {
            this.HandlerClass.Application_Init(sender, e);
        }

        /// <summary>
        /// Handles the Disposed event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Disposed(object sender, EventArgs e)
        {
            this.HandlerClass.Application_Disposed(sender, e);
        }

        /// <summary>
        /// Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            this.HandlerClass.Application_End(sender, e);
        }

        /// <summary>
        /// Handles the EndRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            this.HandlerClass.Application_EndRequest(sender, e);
        }

        /// <summary>
        /// Handles the PostRequestHandlerExecute event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            this.HandlerClass.Application_PostRequestHandlerExecute(sender, e);
        }

        /// <summary>
        /// Handles the PreSendRequestHeaders event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Remove("Server");
            this.HandlerClass.Application_PreSendRequestHeaders(sender, e);
        }

        /// <summary>
        /// Handles the PreSendContent event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_PreSendContent(object sender, EventArgs e)
        {
            this.HandlerClass.Application_PreSendContent(sender, e);
        }

        /// <summary>
        /// Handles the AcquireRequestState event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            this.HandlerClass.Application_AcquireRequestState(sender, e);
        }

        /// <summary>
        /// Handles the ReleaseRequestState event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_ReleaseRequestState(object sender, EventArgs e)
        {
            this.HandlerClass.Application_ReleaseRequestState(sender, e);
        }

        /// <summary>
        /// Handles the ResolveRequestCache event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_ResolveRequestCache(object sender, EventArgs e)
        {
            this.HandlerClass.Application_ResolveRequestCache(sender, e);
        }

        /// <summary>
        /// Handles the UpdateRequestCache event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_UpdateRequestCache(object sender, EventArgs e)
        {
            this.HandlerClass.Application_UpdateRequestCache(sender, e);
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            this.HandlerClass.Application_AuthenticateRequest(sender, e);
        }

        /// <summary>
        /// Handles the AuthorizeRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            this.HandlerClass.Application_AuthorizeRequest(sender, e);
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
            this.HandlerClass.Session_End(sender, e);
        }
    }
}