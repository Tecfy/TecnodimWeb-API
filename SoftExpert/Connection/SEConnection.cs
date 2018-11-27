using System.Web.Configuration;

namespace SoftExpert
{
    public static class SEConnection
    {
        readonly static string Username = WebConfigurationManager.AppSettings["SoftExpert.Username"];
        readonly static string Password = WebConfigurationManager.AppSettings["SoftExpert.Password"];
        readonly static string URL = WebConfigurationManager.AppSettings["SoftExpert.Url"];

        public static SEClient GetConnection()
        {
            SEClient seClient = new SEClient
            {
                Url = URL
            };
            seClient.SetAuthentication(Username, Password);

            return seClient;
        }
    }
}
