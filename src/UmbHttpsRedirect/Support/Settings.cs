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

        /// <summary>
        /// Load settings from configuration file.
        /// </summary>
        public Settings()
        {
            // Load values from config files.
            this.DocTypes = this.ConfigLoadDocTypes();
            this.PageIds = this.ConfigLoadPageIds();
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
        /// Load PageIds from configuration file.
        /// </summary>
        /// <returns>List of PageIds.</returns>
        private List<int> ConfigLoadPageIds()
        {
            // Return empty list if there are no values.
            if (WebConfigurationManager.AppSettings[AppKey_PageIds] == null)
            {
                return new List<int>();
            }

            try
            {
                // Split Page IDs into parts.
                var pageIds = WebConfigurationManager.AppSettings[AppKey_PageIds]
                    .Split(',')
                    .Select(x => x.Trim())
                    .Select(x => Convert.ToInt32(x))
                    .ToList();

                return pageIds;
            }
            catch
            {
                throw new ConfigurationErrorsException("Value for " + AppKey_PageIds + " not correctly specified.");
            }

        }

        /// <summary>
        /// Load DocTypes from configuration file.
        /// </summary>
        /// <returns>List of DocTypes.</returns>
        private List<string> ConfigLoadDocTypes()
        {
            // Return empty list if there are no values.
            if (WebConfigurationManager.AppSettings[AppKey_DocTypes] == null)
            {
                return new List<string>();
            }

            try
            {
                // Split DocTypes into parts.
                var docTypes = WebConfigurationManager.AppSettings[AppKey_DocTypes]
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToList();

                return docTypes;
            }
            catch
            {
                throw new ConfigurationErrorsException("Value for " + AppKey_DocTypes + " not correctly specified.");
            }

        }
    }
}
