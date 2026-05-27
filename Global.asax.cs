using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DotNetEnv;

namespace Gestion_Salle_classe
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Ignore EF model changes verification
            System.Data.Entity.Database.SetInitializer<Gestion_Salle_classe.Models.EMITDbContext>(null);

            try
            {
                string envPath = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, ".env");
                if (System.IO.File.Exists(envPath))
                {
                    DotNetEnv.Env.Load(envPath);
                }
            }
            catch (Exception) { /* Ignored if .env missing */ }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            // Handle OPTIONS preflight requests explicitly
            // (CORS headers are managed by WebApiConfig's EnableCorsAttribute)
            if (Request.HttpMethod == "OPTIONS")
            {
                var origin = Request.Headers["Origin"];
                if (!string.IsNullOrEmpty(origin))
                {
                    Response.Headers.Set("Access-Control-Allow-Origin", origin);
                    Response.Headers.Set("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, X-User-Id, X-User-Role");
                    Response.Headers.Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    Response.Headers.Set("Access-Control-Max-Age", "86400");
                }
                Response.StatusCode = 200;
                Response.End();
            }
        }
    }
}
