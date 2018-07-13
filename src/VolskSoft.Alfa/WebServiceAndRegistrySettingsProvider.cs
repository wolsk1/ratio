namespace VolskSoft.Alfa
{
    using System.Collections.Specialized;

    /// <summary>
    /// Retrieves configuration settings from external web service.
    /// </summary>
    public class WebServiceAndRegistrySettingsProvider : SettingsProviderBase
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
                return "WebServiceAndRegistrySettingsProvider";
            }
        }

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        protected override void MemberwiseInitialize(ConfSection configurationSection)
        {
            var registryProvider = new RegistrySettingsProvider();
            registryProvider.Initialize(configurationSection);

            var url = registryProvider.Values[UrlAttributeName];
            var user = registryProvider.Values[UserAttributeName];
            var password = registryProvider.Values[PasswordAttributeName];

            var section = new ConfSection
            {
                Values = new NameValueCollection()
            };
            section.Values.Add(UrlAttributeName, url);
            section.Values.Add(UserAttributeName, user);
            section.Values.Add(PasswordAttributeName, password);

            var providerWebService = new WebServiceProvider();
            providerWebService.Initialize(section);

            // Get settings
            Values = providerWebService.Values;
        }

    }
}
