using System.Web.Http;
using Owin;
using System.Web.Http.Cors;

namespace EncampmentAnalyticsApi
{
    public class WebApiConfig
    {
        public static void Configure(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            EnableCorsAttribute cors = new EnableCorsAttribute("https://sunrise2018.org,http://encampment.local:4200,https://encampment.azurewebsites.net", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});

            app.UseWebApi(config);
        }
    }
}