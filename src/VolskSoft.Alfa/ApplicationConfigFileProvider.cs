namespace VolskSoft.Alfa
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// Reads configuration from application configuration file's section.
    /// </summary>
    public class ApplicationConfigFileProvider : SettingsProviderBase
    {
        /// <summary>
        /// Name of the Xml node in configuration document to look for.
        /// </summary>
        private const string SectionNameAttributeName = "section-name";

        /// <summary>
        /// Name of the section for later usage.
        /// </summary>
        private string sectionName;

        /// <summary>
        /// Override this in child class to implement custom initialization.
        /// </summary>
        /// <param name="configurationSection">The section.</param>
        protected override void MemberwiseInitialize(ConfSection configurationSection)
        {
            this.sectionName = GetSettingsValue(SectionNameAttributeName);

            if (string.IsNullOrEmpty(this.sectionName))
            {
                throw new InvalidOperationException("Missing parameter \"" + SectionNameAttributeName + "\".");
            }

            NameValueCollection values = ConfigurationManager.GetSection(this.sectionName) as NameValueCollection;
            if (values == null)
            {
                throw new InvalidOperationException("Section \"" + this.sectionName + "\" is not found.");
            }

            SettingsCollection v = new SettingsCollection();
            foreach (string key in values.AllKeys)
            {
                v.Add(key, values.Get(key));
            }

            Values = v;
        }
    }
}