namespace VolskSoft.Ratio.Api
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
        
            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);


            app.Map("", pipeline =>
            {
                pipeline.Use((context, next) =>
                {
                    context.Response.ContentType = "text/plain";
                    return context.Response.WriteAsync("Hello, world.");
                });
            });     
        }
    }
}