namespace VolskSoft.Alfa
{
    using System.Collections.Specialized;

    /// <summary>
    /// Contains information about settings provider read from application configuration file.
    /// </summary>
    public class ConfSection
    {
        public NameValueCollection Values { get; set; }
       
        public bool ShouldReadOnStartup { get; set; }
    }
}
