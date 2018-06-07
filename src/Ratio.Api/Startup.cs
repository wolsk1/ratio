namespace VolskSoft.Ratio.Api
{
    using System;
    using System.IO;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });

            //var wwwroot = Path.Combine(
            //    AppDomain.CurrentDomain.BaseDirectory,
            //    "wwwroot");
            //AutofacInitializer.Initialize();
            //var configurator = Configure(wwwroot, AutofacInitializer.AutofacContainer);
            //configurator(app);            
        }
    }
}