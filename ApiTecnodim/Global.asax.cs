using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Tecnodim.Security;

namespace ApiTecnodim
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // API authorization registration.
            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthorizationHeaderHandler());

            GlobalConfiguration.Configure(WebApiConfig.Register);            
        }
    }
}
