namespace VolskSoft.Alfa
{
    using System;

    /// <summary>
    /// Base class for all settings providers.
    /// </summary>
    public class SettingsProviderBase : ISettingsProvider
    {
        /// <summary>
        /// Reference to configuration section object.
        /// </summary>
        private ConfSection section;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsProviderBase"/> class.
        /// </summary>
        public SettingsProviderBase()
        {
            this.Values = new SettingsCollection();
        }

        /// <summary>
        /// Gets or sets the settings values.
        /// </summary>
        public SettingsCollection Values { get; set; }

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        public virtual string Context
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a setting entry with the given identifier.
        /// </summary>
        /// <param name="key">Identifier of the settings entry.</param>
        /// <returns>A value of the settings entry.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <c>key</c> argument is null.</exception>
        /// <exception cref="NotSupportedException">The property is set and the System.Collections.Hashtable is read-only.-or- The property is set, key does not exist in the collection, and the System.Collections.Hashtable has a fixed size.</exception>
        public object this[string key]
        {
            get
            {
                return this.Values[key];
            }
        }

        /// <summary>
        /// Returns a setting entry with the given identifier.
        /// </summary>
        /// <param name="key">Identifier of the settings entry.</param>
        /// <returns>A value of the settings entry.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <c>key</c> argument is null.</exception>
        public object GetValue(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the settings contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the settings.</param>
        /// <returns><c>true</c> if the settings contains an element with the specified key; otherwise, <c>false</c></returns>
        public bool ContainsKey(string key)
        {
            foreach (string s in this.Values.Keys)
            {
                if (s.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reload provider data 
        /// </summary>
        public void Reload()
        {
            this.Values.Clear();
            this.MemberwiseInitialize(this.section);
        }

        /// <summary>
        /// Method is design to perform any startup initialization tasks.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        public void Initialize(ConfSection configurationSection)
        {
            this.section = configurationSection;
            this.MemberwiseInitialize(this.section);
        }

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="configurationSection">The section.</param>
        protected virtual void MemberwiseInitialize(ConfSection configurationSection)
        {
        }

        /// <summary>
        /// Gets the settings value either from section or settings context.
        /// </summary>
        /// <param name="key">The name of the settings key to get value from.</param>
        /// <returns>Value of the key</returns>
        /// <exception cref="InvalidOperationException">If key is not found.</exception>
        /// <remarks>Method is primary used by providers to look for initialization parameters needed by provider.</remarks>
        protected string GetSettingsValue(string key)
        {
            string value = this.section.Values.Get(key);

            if (string.IsNullOrEmpty(value))
            {
                if (Settings.Contains(key))
                {
                    value = Settings.GetString(key);
                }
                else
                {
                    throw new InvalidOperationException("Missing parameter \"" + key + "\".");
                }
            }

            return value;
        }

        /// <summary>
        /// Determines whether section or settings context contains specified key.
        /// </summary>
        /// <param name="key">The name of the settings key.</param>
        /// <returns>
        /// 	<c>true</c> if section or settings context contains specified key; otherwise, <c>false</c>.
        /// </returns>
        protected bool ContainsSettingsValue(string key)
        {
            string value = this.section.Values.Get(key);

            if (string.IsNullOrEmpty(value))
            {
                if (!Settings.Contains(key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}