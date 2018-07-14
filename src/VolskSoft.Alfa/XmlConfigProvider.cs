namespace VolskSoft.Alfa
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Reads configuration information from external Xml file.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Use in non static manner.")]
    public class XmlConfigProvider : SettingsProviderBase
    {
        /// <summary>
        /// Name of the file path attribute.
        /// </summary>
        private const string FilePathAttributeName = "file-path";

        /// <summary>
        /// File path value.
        /// </summary>
        private string filePath;

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="node">The config node.</param>
        /// <returns>Returns a collection of all settings keys and values.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Use in non static manner.")]
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "Will be refactored later.")]
        public SettingsCollection ReadSettings(XmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            SettingsCollection settings = new SettingsCollection();
            XmlNodeList keys = node.ChildNodes;

            foreach (XmlNode key in keys)
            {
                if (key.NodeType != XmlNodeType.Comment)
                settings.Add(key.Attributes.GetNamedItem("key").InnerText,
                             key.Attributes.GetNamedItem("value").InnerText);
            }

            return settings;
        }

        /// <summary>
        /// Method is design to perform any startup initialization tasks.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        protected override void MemberwiseInitialize(ConfSection configurationSection)
        {
            this.filePath = GetSettingsValue(FilePathAttributeName);

            if (!File.Exists(this.filePath))
            {
                throw new InvalidOperationException("File \"" + this.filePath + "\" does not exist.");
            }

            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(this.filePath);

            // TODO: validate against XSD schema
            Values = this.ReadSettings(configDoc.SelectSingleNode("/configuration"));
        }
    }
}