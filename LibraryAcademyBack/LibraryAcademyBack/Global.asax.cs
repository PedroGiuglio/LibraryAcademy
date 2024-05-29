using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.SqlClient;
using System.Net;

namespace LibraryAcademyBack
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Configuración CORS a nivel de aplicación
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:4200");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Verificar si hay un contexto de usuario y si el usuario está autenticado
            if (Context.User != null && Context.User.Identity != null && Context.User.Identity.IsAuthenticated)
            {
                // El usuario está autenticado
                string username = Context.User.Identity.Name;
                // Puedes realizar más acciones aquí si es necesario
            }
        }

      

    }

}
