using System.Web.Http;
using ApiTecnodim.Security;

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
