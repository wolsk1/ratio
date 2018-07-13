namespace VolskSoft.Alfa
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Provides clients with configuration settings information.
    /// Use this provider to read settings from registry export files *.reg.
    /// Recomend to use in UnitTest where we do not have (and did not need) access to windows registry.
    /// </summary>
    public class RegistryFileSettingsProvider : ISettingsProvider
    {
        /// <summary>
        /// File name of the 
        /// </summary>
        private string fileName;

        /// <summary>
        /// Values of entries in config section.
        /// </summary>
        private SettingsCollection values;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryFileSettingsProvider"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public RegistryFileSettingsProvider(string fileName)
        {
            this.fileName = fileName;
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (!this.Ping())
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  "File \"{0}\" does not exists",
                                                                  fileName));
            }
        }

        /// <summary>
        /// Gets provider's context string. (Used for visualization in error messages).
        /// </summary>
        public string Context
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "File:{0}", this.fileName);
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public SettingsCollection Values
        {
            get
            {
                if (this.values == null)
                {
                    this.values = new SettingsCollection();
                    foreach (DictionaryEntry entry in this.Settings)
                    {
                        this.values.Add(entry.Key.ToString(), entry.Value.ToString());
                    }
                }

                return this.values;
            }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        private Hashtable Settings
        {
            get
            {
                return this.GetSettings();
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">Name of the settings key.</param>
        /// <returns>A value of the settings entry.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <c>key</c> argument is null.</exception>
        public object this[string key]
        {
            get
            {
                return this.Settings[key];
            }
        }

        /// <summary>
        /// Reload provider data 
        /// </summary>
        public void Reload()
        {
            this.values = null;
        }

        /// <summary>
        /// Initializes the specified section.
        /// </summary>
        /// <param name="configurationSection">The section.</param>
        public void Initialize(ConfSection configurationSection)
        {
            this.values = new SettingsCollection();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The settings key name.</param>
        /// <returns>Returns value of the settings key by specified name.</returns>
        public object GetValue(string key)
        {
            return this.Settings[key];
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The name of the settings key to test for..</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return this.Settings.Contains(key);
        }

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <returns>Returns <c>true</c> if service provider can be pinged; otherwise <c>false</c>.</returns>
        private bool Ping()
        {
            return File.Exists(this.fileName);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns>Returns <see cref="Hashtable"/> of settings values.</returns>
        private Hashtable GetSettings()
        {
            Hashtable settingsHashtable = new Hashtable();
            StreamReader sr = new StreamReader(this.fileName);

            try
            {
                // Read the first line of text
                string line = sr.ReadLine();

                if (line != null)
                {
                    // Ignore first line and read again
                    line = sr.ReadLine();
                }

                // Continue to read until you reach end of file
                while (line != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        // Read the next line
                        line = sr.ReadLine();
                        continue;
                    }

                    if (line[0] == '[')
                    {
                        // Read the next line
                        line = sr.ReadLine();
                        continue;
                    }

                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index - 1);
                    key = key.Trim().Trim('"');

                    if (index == line.Length - 1)
                    {
                        settingsHashtable.Add(key, "");
                    }
                    else
                    {
                        string value = line.Substring(index + 1, line.Length - index - 1);
                        value = value.Trim().Trim('"');
                        if (value.Length > 6
                            && value.Substring(0, 6) == "dword:")
                        {
                            value = value.Remove(0, 6);
                            value =
                                long.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture).ToString(
                                    CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            value = value.Replace("\\\"", "\"");
                        }

                        settingsHashtable.Add(key, value);
                    }

                    // Read the next line
                    line = sr.ReadLine();
                }
            }
            finally
            {
                // close the file
                sr.Close();
            }

            return settingsHashtable;
        }
    }
}