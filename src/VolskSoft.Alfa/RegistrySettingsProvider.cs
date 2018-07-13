namespace VolskSoft.Alfa
{
    using System;    
    using System.Globalization;
    using Microsoft.Win32;

    /// <summary>
    /// Provides clients with configuration settings information.
    /// </summary>
    public class RegistrySettingsProvider : SettingsProviderBase
    {
        /// <summary>
        /// Value of the attribute name that contains registry path.
        /// </summary>
        private const string PathAttributeName = "registry-path";

        /// <summary>
        /// Value of the registry root node.
        /// </summary>
        private string configRoot;

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        public override string Context
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                                     "{0}:{1}",
                                     PathAttributeName.ToUpper(CultureInfo.InvariantCulture),
                                     this.ConfigRootNode);
            }
        }

        /// <summary>
        /// Gets or sets registry node to retrieve settings from.
        /// </summary>
        public string ConfigRootNode
        {
            get
            {
                return this.configRoot;
            }

            set
            {
                this.configRoot = value;
            }
        }

        /// <summary>
        /// Gets root node of configuration.
        /// </summary>
        private RegistryKey RootConfigNode
        {
            get
            {
                string rootNodeName = this.ConfigRootNode;
                RegistryKey result = Registry.LocalMachine;

                while (rootNodeName.IndexOf('/') != -1)
                {
                    int delimiterPosition = rootNodeName.IndexOf('/');

                    result = result.OpenSubKey(rootNodeName.Substring(0, delimiterPosition));
                    rootNodeName = rootNodeName.Substring(delimiterPosition + 1);
                }

                // failed to open key - probably does not exist
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to open registry key: \"" + this.ConfigRootNode + "\"");
                }

                result = result.OpenSubKey(rootNodeName);

                return result;
            }
        }

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="configurationSection">The section.</param>
        protected override void MemberwiseInitialize(ConfSection configurationSection)
        {
            if (string.IsNullOrEmpty(this.ConfigRootNode))
            {
                this.ConfigRootNode = GetSettingsValue(PathAttributeName);
            }

            Values = this.LoadSettings();
        }

        /// <summary>
        /// Reads the content of the node.
        /// </summary>
        /// <param name="hiveKey">The hive key.</param>
        /// <param name="collection">The collection.</param>
        private static void ReadNodeContent(RegistryKey hiveKey, ref SettingsCollection collection)
        {
            foreach (string key in hiveKey.GetValueNames())
            {
                string value = Convert.ToString(hiveKey.GetValue(key), CultureInfo.InvariantCulture);
                collection.Add(key, value);
            }
        }

        /// <summary>
        /// Reads all configuration settings from Windows registry.
        /// </summary>
        /// <returns>Read settings as <see cref="Hashtable"/>.</returns>
        private SettingsCollection LoadSettings()
        {
            SettingsCollection result = new SettingsCollection();
            this.ReadNodeContentRecursive(this.RootConfigNode, ref result);
            return result;
        }

        /// <summary>
        /// Reads the node content recursive.
        /// </summary>
        /// <param name="hiveKey">The hive key.</param>
        /// <param name="collection">The collection.</param>
        private void ReadNodeContentRecursive(RegistryKey hiveKey, ref SettingsCollection collection)
        {
            ReadNodeContent(hiveKey, ref collection);

            string[] hives = hiveKey.GetSubKeyNames();
            foreach (string hive in hives)
            {
                this.ReadNodeContentRecursive(hiveKey.OpenSubKey(hive), ref collection);
            }
        }
    }
}