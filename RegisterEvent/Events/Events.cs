using DataEF.DataAccess;
using System.Web.Configuration;

namespace RegisterEvent.Events
{
    public class Events
    {
        public void SaveRegisterEvent(string userId, string identifier, string type, string source, string text)
        {
            if (WebConfigurationManager.AppSettings["Repository.SaveRegisterEvent"].ToString() == "true")
            {
                using (var context = new DBContext())
                {
                    RegisterEvents registerEvent = new RegisterEvents() { UserId = userId, Identifier = identifier, Type = type, Source = source, Text = text };
                    context.RegisterEvents.Add(registerEvent);
                    context.SaveChanges();
                }
            }
        }
    }
}
