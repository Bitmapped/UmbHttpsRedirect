using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Configuration;

namespace UmbHttpsRedirect.Support
{
    public class Settings
    {
        // Define constants.
        private const string AppKey_DocTypes = "HttpsRedirect:DocTypes";
        private const string AppKey_PageIds = "HttpsRedirect:PageIds";
        private const string AppKey_Templates = "HttpsRedirect:Templates";
        private const string AppKey_ForceHttp = "HttpsRedirect:ForceHttp";
        private const string AppKey_HttpPort = "HttpsRedirect:HttpPort";
        private const string AppKey_HttpsPort = "HttpsRedirect:HttpsPort";
        private const string AppKey_UseTemporaryRedirects = "HttpsRedirect:UseTemporaryRedirects";

        /// <summary>
        /// Load settings from configuration file.
        /// </summary>
        public Settings()
        {
            // Load values from config files.
            this.DocTypes = this.ConfigLoadStringList(AppKey_DocTypes);
            this.Templates = this.ConfigLoadStringList(AppKey_Templates);
            this.PageIds = this.ConfigLoadIntList(AppKey_PageIds);
            this.ForceHttp = this.ConfigLoadBool(AppKey_ForceHttp, false);
            this.UseTemporaryRedirects = this.ConfigLoadBool(AppKey_UseTemporaryRedirects, false);
            this.HttpPort = this.ConfigLoadNullableInt(AppKey_HttpPort);
            this.HttpsPort = this.ConfigLoadNullableInt(AppKey_HttpsPort);
        }

        /// <summary>
        /// Page IDs to force to be HTTPS.
        /// </summary>
        public IEnumerable<int> PageIds { get; private set; }

        /// <summary>
        /// Document types to force to be HTTPS.
        /// </summary>
        public IEnumerable<string> DocTypes { get; private set; }

        /// <summary>
        /// Templates to force to be HTTPS.
        /// </summary>
        public IEnumerable<string> Templates { get; private set; }

        /// <summary>
        /// Force HTTP is page is not specifically set to be HTTPS. Default/false is not to force redirect to HTTP.
        /// </summary>
        public bool ForceHttp { get; private set; }

        /// <summary>
        /// Use temporary 302 redirects rather than permanent 301 redirects.
        /// </summary>
        public bool UseTemporaryRedirects { get; private set; }

        /// <summary>
        /// Optional port to use for redirecting to HTTP.
        /// </summary>
        public int? HttpPort { get; private set; }

        /// <summary>
        ///  Optional prot to use for redirecting to HTTPS.
        /// </summary>
        public int? HttpsPort { get; private set; }

        /// <summary>
        /// Load comma-separated list of ints from configuration file.
        /// </summary>
        /// /// <param name="key">Key to load.</param>
        /// <returns>List of values.</returns>
        private List<int> ConfigLoadIntList(string key)
        {
            // Return empty list if there are no values.
            if (WebConfigurationManager.AppSettings[key] == null)
            {
                return new List<int>();
            }

            try
            {
                // Split Page IDs into parts.
                var pageIds = WebConfigurationManager.AppSettings[key]
                    .Split(',')
                    .Select(x => x.Trim())
                    .Select(x => int.Parse(x))
                    .ToList();

                return pageIds;
            }
            catch
            {
                throw new ConfigurationErrorsException(String.Format("Value for {0} not correctly specified.", key));
            }

        }

        /// <summary>
        /// Load comma-separated list of strings from configuration file.
        /// </summary>
        /// <param name="key">Key to load.</param>
        /// <returns>List of values.</returns>
        private List<string> ConfigLoadStringList(string key)
        {
            // Return empty list if there are no values.
            if (WebConfigurationManager.AppSettings[key] == null)
            {
                return new List<string>();
            }

            try
            {
                // Split list into parts.
                var values = WebConfigurationManager.AppSettings[key]
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToList();

                return values;
            }
            catch
            {
                throw new ConfigurationErrorsException(String.Format("Value for {0} not correctly specified.", key));
            }
        }

        /// <summary>
        /// Load configuration value for boolean.
        /// </summary>
        /// <param name="key">Key to load.</param>
        /// <param name="defaultValue">Default value to return if key is not specified.</param>
        /// <returns>True/false.</returns>
        private bool ConfigLoadBool(string key, bool defaultValue)
        {
            // If key was not set, default to false.
            if (WebConfigurationManager.AppSettings[key] == null)
            {
                return defaultValue;
            }

            // Value was set. Test it.
            try
            {
                return Boolean.Parse(WebConfigurationManager.AppSettings[key]);
            }
            catch
            {
                throw new ConfigurationErrorsException(String.Format("Value for {0} not correctly specified.", key));
            }
        }

        /// <summary>
        /// Load configuration value for nullable integer.
        /// </summary>
        /// <param name="key">Key to load.</param>
        /// <returns>Integer or null.</returns>
        private int? ConfigLoadNullableInt(string key)
        {
            // If key was not set, return null.
            // If key was not set, default to false.
            if (WebConfigurationManager.AppSettings[key] == null)
            {
                return null;
            }

            // Value was set. Test it.
            try
            {
                return Int32.Parse(WebConfigurationManager.AppSettings[key]);
            }
            catch
            {
                throw new ConfigurationErrorsException(String.Format("Value for {0} not correctly specified.", key));
            }
        }
    }
}
