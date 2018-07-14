namespace VolskSoft.Alfa
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using VolskSoft.Bibliotheca;

    /// <summary>
    /// Class provides access to settings store.
    /// Implements Composite pattern therefore can contain various internal providers.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Attribute name of the config section which contains name of the settings provider.
        /// </summary>
        private const string ConfigSection = "settings-provider";

        /// <summary>
        /// Attribute name of the cofnig section which contains value of the settings provider type.
        /// </summary>
        private const string ProviderTypeKey = "provider-type";

        /// <summary>
        /// Syncs access to provider creation moment.
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// Cotnains list of all providers.
        /// </summary>
        private static Collection<ISettingsProvider> providers;

        /// <summary>
        /// Field is used to provide error message with all corresponding providers and appropriate contexts
        /// </summary>
        private static string providersContext = string.Empty;

        /// <summary>
        /// Contains list of all settings keys and its values.
        /// </summary>
        private static SettingsCollection settings;

        /// <summary>
        /// Adds the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <remarks>Provider's initialize method is <b>not</b> called.</remarks>
        public static void AddProvider(ISettingsProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            TestInstance();

            providers.Add(provider);
            CopyProviderData(provider);
        }

        /// <summary>
        /// Determines whether the settings contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the settings.</param>
        /// <returns><c>true</c> if the settings contains an element with the specified key; otherwise, <c>false</c></returns>
        public static bool Contains(string key)
        {
            TestInstance();

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            string localKey = key.ToUpperInvariant();

            foreach (string k in settings.AllKeys)
            {
                if (localKey.Equals(k))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns key value from settings as boolean.
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <returns>A key value from configuration settings as boolean (1 means <c>true</c>).</returns>
        public static bool GetBoolean(string key)
        {
            return Convert.ToBoolean(GetInt32(key));
        }

        /// <summary>
        /// Returns a key value from settings.
        /// If no value was found return the default value
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <param name="defaultValue">Default value if no key was found</param>
        /// <returns>Returns a settings key value as boolean.</returns>
        public static bool GetBoolean(string key, bool defaultValue)
        {
            TestInstance();

            try
            {
                return GetBoolean(key);
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns key value from settings as decimal.
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <returns>A key value from configuration settings as decimal.</returns>
        public static decimal GetDecimal(string key)
        {
            return Convert.ToDecimal(GetString(key), Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Returns a key value from settings.
        /// If no value was found return the default value
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <param name="defaultValue">Default value if no key was found</param>
        /// <returns>Returns a settings key value as decimal.</returns>
        public static decimal GetDecimal(string key, decimal defaultValue)
        {
            TestInstance();

            try
            {
                return GetDecimal(key);
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns key value from settings as guid.
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <returns>A key value from configuration settings as guid.</returns>
        public static Guid GetGuid(string key)
        {
            Guid guid = new Guid(GetString(key));
            return guid;
        }

        /// <summary>
        /// Returns key value from settings as integer.
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <returns>A key value from configuration settings as integer.</returns>
        public static int GetInt32(string key)
        {
            return Convert.ToInt32(GetValue(key).ToString(), Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Returns a key value from settings.
        /// If no value was found return the default value
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <param name="defaultValue">Default value if no key was found</param>
        /// <returns>Returns a settings key value as integer.</returns>
        public static int GetInt32(string key, int defaultValue)
        {
            TestInstance();

            try
            {
                return GetInt32(key);
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns key value from settings as string.
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <returns>A key value from configuration settings as string.</returns>
        public static string GetString(string key)
        {
            return GetValue(key).ToString();
        }

        /// <summary>
        /// Returns a key value from settings.
        /// If no value was found return the default value
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <param name="defaultValue">Default value if no key was found</param>
        /// <returns>Returns a settings key value as string.</returns>
        public static string GetString(string key, string defaultValue)
        {
            TestInstance();

            try
            {
                return GetString(key);
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns a key value from settings
        /// </summary>
        /// <param name="key">The key of the seting value to get.</param>
        /// <returns>Returns a value of settings key by specified name.</returns>
        /// <exception cref="ArgumentException">If key does not exist.</exception>
        public static object GetValue(string key)
        {
            TestInstance();

            if (Contains(key))
            {
                return settings.Get(key);
            }

            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture,
                                                         "Key named \"{0}\" does not exist in configuration. Following providers are configured:{1}",
                                                         key,
                                                         Environment.NewLine + providersContext));
        }

        /// <summary>
        /// Returns a key value from settings.
        /// If no value was found return the default value
        /// </summary>
        /// <param name="key">Name of the key to access.</param>
        /// <param name="defaultValue">Default value if no key was found</param>
        /// <returns>Retuns a value of settings key by specified name if found; otherwise <paramref name="defaultValue"/>.</returns>
        public static object GetValue(string key, object defaultValue)
        {
            TestInstance();

            try
            {
                return GetValue(key);
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Re-read all configuration values from providers.
        /// </summary>
        public static void Refresh()
        {
            if (providers == null)
            {
                throw new InvalidOperationException("Providers collection is null.");
            }

            // If there are no any providers than clean up settings
            lock (syncRoot)
            {
                settings = new SettingsCollection();
                foreach (ISettingsProvider provider in providers)
                {
                    CopyProviderData(provider);
                }
            }
        }

        /// <summary>
        /// Reload all providers with data from data sources and rebuild configurations list
        /// </summary>
        public static void Reload()
        {
            if (providers == null)
            {
                throw new InvalidOperationException("Providers collection is null.");
            }

            lock (syncRoot)
            {
                settings = new SettingsCollection();
                foreach (ISettingsProvider provider in providers)
                {
                    provider.Reload();
                    CopyProviderData(provider);
                }
            }
        }

        /// <summary>
        /// Removes all providers and keys from the settings.
        /// </summary>
        public static void RemoveAllProviders()
        {
            // we need to read all providers at its content (as this is done in first request - bad design).
            TestInstance();
            settings.Clear();
            providers.Clear();
        }

        /// <summary>
        /// Remove the provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public static void RemoveProvider(ISettingsProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (!providers.Contains(provider))
            {
                throw new MalfunctionException("There are no given provider in provider list.");
            }

            TestInstance();

            providers.Remove(provider);
            Refresh();            
        }

        /// <summary>
        /// Copies the provider's data.
        /// </summary>
        /// <param name="provider">The provider.</param>
        private static void CopyProviderData(ISettingsProvider provider)
        {
            foreach (string key in provider.Values.AllKeys)
            {
                settings.Add(key, provider.Values.Get(key));
            }
        }

        /// <summary>
        /// Creates ISettingsProvider collection based on application configuration.
        /// </summary>
        private static void GetProviders()
        {
            var section = ConfigurationManager.GetSection(ConfigSection) as ConfSection;

            if (section == null)
            {
                // Configuration section is null - that means there is no configuration - it's not an exception.
                return;
            }

            TestInstance();

            // Add values from settings-provider section to settings collection
            // and skip provider-type values
            foreach (string key in section.Values.AllKeys)
            {
                if (key == ProviderTypeKey)
                {
                    continue;
                }

                settings.Add(key, section.Values[key]);
            }

            string[] providers = section.Values.GetValues(ProviderTypeKey);

            if (providers != null)
            {
                foreach (string providerType in providers)
                {
                    if (string.IsNullOrEmpty(providerType))
                    {
                        throw new InvalidOperationException("Missing configuration attribute \"" + ProviderTypeKey);
                    }

                    Type t = Type.GetType(providerType);
                    ISettingsProvider provider = (ISettingsProvider)Activator.CreateInstance(t);

                    if (section.ShouldReadOnStartup)
                    {
                        provider.Initialize(section);
                        providersContext = providersContext +
                                           string.Format(CultureInfo.InvariantCulture,
                                                         "{0}{1}    {2}",
                                                         provider.GetType().FullName,
                                                         Environment.NewLine,
                                                         provider.Context)
                                           + Environment.NewLine;
                    }

                    AddProvider(provider);
                }
            }
        }

        /// <summary>
        /// Tests the instance.
        /// </summary>
        private static void TestInstance()
        {
            if (settings == null)
            {
                lock (syncRoot)
                {
                    settings = new SettingsCollection();
                    providers = new Collection<ISettingsProvider>();

                    try
                    {
                        GetProviders();
                    }
                    catch (ConfigurationErrorsException)
                    {
                        /*
                         * if during executing there will be no configuration context
                         * empty settings provider will be created to provider and empty settings store there will be no keys
                         */
                    }
                }
            }
        }
    }
}