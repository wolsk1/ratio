namespace VolskSoft.Alfa
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Threading;    

    /// <summary>
    /// Retrieves configuration settings from external web service.
    /// </summary>
    public class WebServiceProvider : SettingsProviderBase
    {
        /// <summary>
        /// Password attrbute name.
        /// </summary>
        private const string PasswordAttributeName = "CONFIG_WEBSERVICES_PASSWORD";

        /// <summary>
        /// URI attrbiute name.
        /// </summary>
        private const string UrlAttributeName = "CONFIG_WEBSERVICES_URL";

        /// <summary>
        /// User name attribute name.
        /// </summary>
        private const string UserAttributeName = "CONFIG_WEBSERVICES_USER";

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        /// <value></value>
        public override string Context
        {
            get
            {
                return "WebServiceProvider";
            }
        }

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        protected override void MemberwiseInitialize(ConfSection configurationSection)
        {
            var url = GetSettingsValue(UrlAttributeName);
            var user = GetSettingsValue(UserAttributeName);
            var password = GetSettingsValue(PasswordAttributeName);

            var providerWebService = new Provider();
            var cred = new NetworkCredential(user, password);

            providerWebService.PreAuthenticate = true;
            providerWebService.Credentials = cred;
            providerWebService.Url = url;

            // Try to get settings.
            PingConfigWebServices(providerWebService);

            // Get settings
            Values = GetSettings(providerWebService);
        }

        /// <summary>
        /// Make call to a webservice to get all configuration settings.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>Get settings collection <see cref="SettingsCollection"/>.</returns>
        private static SettingsCollection GetSettings(Provider provider)
        {
            var settingsData = provider.GetSettings();

            // Convert to SettingsCollection
            var collection = new SettingsCollection();
            foreach (var setting in settingsData)
            {
                collection.Add(setting.Key, setting.Value);
            }

            return collection;
        }

        /// <summary>
        /// Tests if configuration web services is alive.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns><c>true</c> if ping successed, otherwise <c>false</c>.</returns>
        private static bool Ping(Provider provider)
        {
            try
            {
                provider.GetSettingsVersion();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Pings the config web services.
        /// </summary>
        /// <param name="provider">The provider.</param>
        private static void PingConfigWebServices(Provider provider)
        {
            int count = 1;

            while (count < 10)
            {
                Trace.WriteLine("Trying to ping configuration web services...");
                Trace.Flush();
                if (Ping(provider))
                {
                    Trace.WriteLine("Configuration web services touch succeed.");
                    Trace.Flush();
                    return;
                }

                Trace.WriteLine(
                    string.Format(CultureInfo.InvariantCulture,
                                  "Error - Cannot ping configuration web services ({0}). Retrying after 30 seconds....",
                                  count));
                Trace.Flush();
                count++;
                Thread.Sleep(30000);
            }

            throw new ConfigurationErrorsException("Cannot ping configuration web services. Retry count (10) exceeded.");
        }
    }
}