namespace VolskSoft.Ratio.ServiceHost
{
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            //TODO run service locally
            ServiceBase.Run(new ServiceBase[]
            {
                new ServiceHost()
            });
        }
    }
}
