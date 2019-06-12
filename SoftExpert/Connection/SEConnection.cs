using System.Net;
using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEConnection
    {
        readonly static string Username = WebConfigurationManager.AppSettings["SoftExpert.Username"];
        readonly static string Password = WebConfigurationManager.AppSettings["SoftExpert.Password"];
        readonly static string URL = WebConfigurationManager.AppSettings["SoftExpert.Url"];
        readonly static string URLAdm = WebConfigurationManager.AppSettings["SoftExpert.UrlAdm"];

        public static SEClient GetConnection()
        {
            SEClient seClient = new SEClient { Url = URL};
            seClient.SetAuthentication(Username, Password);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            return seClient;
        }

        public static SEClient GetConnection(string username, string password)
        {
            SEClient seClient = new SEClient { Url = URL };
            seClient.SetAuthentication(username, Password);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            return seClient;
        }

        public static SEAdministration GetConnectionAdm()
        {
            SEAdministration seAdministration = new SEAdministration { Url = URLAdm };
            seAdministration.SetAuthentication(Username, Password);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            return seAdministration;
        }
    }
}
