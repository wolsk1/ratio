namespace VolskSoft.Ratio.Host
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<Api.Startup>("http://localhost:9000"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}
