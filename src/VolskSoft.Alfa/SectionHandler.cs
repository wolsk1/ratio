namespace VolskSoft.Alfa
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Xml;

    /// <summary>
    /// Handles all settings provider configuration section in application configuration file.
    /// </summary>
    public class SectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Configuration section.</param>
        /// <returns>
        /// The created <see cref="ConfigurationSection"/> object.
        /// </returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }

            var configuration = new ConfSection();
            var reader = new XmlConfigProvider();
            NameValueCollection settings = reader.ReadSettings(section);
            configuration.Values = settings;

            XmlNode initAttribute = section.Attributes.GetNamedItem("init");
            configuration.ShouldReadOnStartup = true;

            if (initAttribute != null)
            {
                string initMode = initAttribute.InnerText;

                switch (initMode)
                {
                    case "auto":
                        configuration.ShouldReadOnStartup = true;
                        break;
                    case "ondemand":
                        configuration.ShouldReadOnStartup = false;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown initialization mode \"" + initMode + "\"");
                }
            }

            return configuration;
        }
    }
}