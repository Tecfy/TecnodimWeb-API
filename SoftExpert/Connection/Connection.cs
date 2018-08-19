using System.Web.Configuration;

namespace SoftExpert
{
    public static class Connection
    {
        readonly static string Username = WebConfigurationManager.AppSettings["SoftExpert.Connection.Connection.Username"];
        readonly static string Password = WebConfigurationManager.AppSettings["SoftExpert.Connection.Connection.Password"];
        readonly static string URL = WebConfigurationManager.AppSettings["SoftExpert.Connection.Connection.Url"];

        public static SEClient GetConnection()
        {
            SEClient seClient = new SEClient();
            seClient.Url = URL;
            seClient.SetAuthentication(Username, Password);

            return seClient;
        }
    }
}
