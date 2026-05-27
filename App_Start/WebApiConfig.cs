using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;

namespace Gestion_Salle_classe
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Activer CORS pour toutes les origines localhost (dev)
            var cors = new EnableCorsAttribute(
                "http://localhost:3000,http://localhost:3001,http://localhost:4000,https://localhost:3000,https://localhost:3001",
                "*",
                "GET,POST,PUT,DELETE,OPTIONS"
            );
            config.EnableCors(cors);

            // Serializer settings pour éviter les références circulaires EF
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            // Routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
