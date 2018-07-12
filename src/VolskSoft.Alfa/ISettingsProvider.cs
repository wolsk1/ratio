namespace VolskSoft.Alfa
{
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        SettingsCollection Values { get; }

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        string Context { get; }

        /// <summary>
        /// Returns a setting entry with the given identifier.
        /// </summary>
        /// <param name="key">Identifier of the settings entry.</param>
        /// <returns>A value of the settings entry.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <c>key</c> argument is null.</exception>
        object this[string key] { get; }

        /// <summary>
        /// Returns a setting entry with the given identifier.
        /// </summary>
        /// <param name="key">Identifier of the settings entry.</param>
        /// <returns>A value of the settings entry.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <c>key</c> argument is null.</exception>
        object GetValue(string key);

        /// <summary>
        /// Determines whether the settings contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the settings.</param>
        /// <returns><c>true</c> if the settings contains an element with the specified key; otherwise, <c>false</c></returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Reload provider data 
        /// </summary>
        void Reload();

        /// <summary>
        /// Method is design to perform any startup initialization tasks.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        void Initialize(ConfigurationSection configurationSection);
    }
}