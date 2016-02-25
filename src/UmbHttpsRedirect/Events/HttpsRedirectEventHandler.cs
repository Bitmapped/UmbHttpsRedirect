using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UmbHttpsRedirect.Support;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace UmbHttpsRedirect.Events
{
    public class HttpsRedirectEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// Configuration settings.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        /// Constructor to load settings.
        /// </summary>
        public HttpsRedirectEventHandler()
        {
            // Load settings from config file.
            this.settings = new Settings();
        }

        /// <summary>
        /// Register event handler on start.
        /// </summary>
        /// <param name="httpApplicationBase">Umbraco application.</param>
        /// <param name="applicationContext">Application context.</param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            PublishedContentRequest.Prepared += PublishedContentRequest_Prepared;
        }

        /// <summary>
        /// Event handler to redirect traffic to HTTPS.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        private void PublishedContentRequest_Prepared(object sender, EventArgs e)
        {
            // Get request.
            var request = sender as PublishedContentRequest;
            var context = HttpContext.Current;

            // If the response is invalid, the page doesn't exist, or will be changed already, don't do anything more.
            if ((request == null) || (!request.HasPublishedContent) || (request.Is404) || (request.IsRedirect) || (request.ResponseStatusCode > 0))
            {
                // Log for debugging.
                LogHelper.Debug<HttpsRedirectEventHandler>("Stopping HttpsRedirect for requested URL {0} because request was null ({1}), there was no published content ({2}), was 404 ({3}), was a redirect ({4}), or status code ({5}) was already set.",
                    () => context.Request.Url.AbsolutePath,
                    () => (request == null),
                    () => (!request.HasPublishedContent),
                    () => (request.Is404),
                    () => (request.IsRedirect),
                    () => request.ResponseStatusCode);

                return;
            }

            // Determine if page should be redirected. Check page ID, property, and document type.
            if (request.PublishedContent.GetPropertyValue<bool>("umbHttpsRedirect", false) || settings.PageIds.Contains(request.PublishedContent.Id) || settings.DocTypes.Contains(request.PublishedContent.DocumentTypeAlias))
            {
                // Page should be HTTPS. See if it is already HTTPS.
                if (!context.Request.IsSecureConnection)
                {
                    // Redirect to HTTPS.
                    Redirect(context, true);
                }
            }
            else
            {
                // Page should be HTTP. If it is HTTPS and we need to force HTTP, redirect.
                if (context.Request.IsSecureConnection && settings.ForceHttp)
                {
                    // Redirect to HTTP.
                    Redirect(context, false);
                }
            }
        }

        private void Redirect(HttpContext context, bool redirectToHttps)
        {
            // Get the existing Url.
            var existingUrl = context.Request.Url;

            // Construct new url.
            var uriBuilder = new UriBuilder()
            {
                Host = existingUrl.Host,
                Path = existingUrl.AbsolutePath,
                Query = existingUrl.Query,
                Fragment = existingUrl.Fragment
            };

            // Determine if we are redirecting to HTTPs.
            if (redirectToHttps)
            {
                uriBuilder.Scheme = "https";
                uriBuilder.Port = settings.HttpsPort ?? -1; // Use default port (-1) if port was not specified.
            }
            else
            {
                uriBuilder.Scheme = "http";
                uriBuilder.Port = settings.HttpPort ?? -1; // Use default port (-1) if port was not specified.
            }

            // Log that we are redirecting.
            LogHelper.Debug<HttpsRedirectEventHandler>("Redirecting from {0} to {1} using {2} redirect.",
                            () => existingUrl.AbsoluteUri,
                            () => uriBuilder.Uri.AbsoluteUri,
                            () => (settings.UseTemporaryRedirects ? "temporary" : "permanent"));

            // Perform redirect.
            if (settings.UseTemporaryRedirects)
            {
                context.Response.Redirect(uriBuilder.Uri.AbsoluteUri);
            }
            else
            {
                context.Response.RedirectPermanent(uriBuilder.Uri.AbsoluteUri);
            }
        }
    }
}
