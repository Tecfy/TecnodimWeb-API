using Model.In;
using RegisterEvent.Events;
using System;
using System.Configuration;
using System.Threading;

namespace SoftExpert
{
    public static class SEUserPermission
    {
        public static bool SEUserPermissionUpdate(ECMUserPermissionIn eCMUserPermissionIn)
        {
            Events events = new Events();
            SEGeneric sEGeneric = SEConnection.GetConnectionGn();
            SEAdministration sEAdministration = SEConnection.GetConnectionAdm();

            if (string.IsNullOrEmpty(eCMUserPermissionIn.uninassau))
            {
                AddUserDepartment(eCMUserPermissionIn.iduser, "001-031", "001-031", sEAdministration, events, eCMUserPermissionIn.userId, eCMUserPermissionIn.key, 1);
            }

            if (string.IsNullOrEmpty(eCMUserPermissionIn.univeritas))
            {
                AddUserDepartment(eCMUserPermissionIn.iduser, "028-006", "028-006", sEAdministration, events, eCMUserPermissionIn.userId, eCMUserPermissionIn.key, 1);
            }

            if (string.IsNullOrEmpty(eCMUserPermissionIn.unama))
            {
                AddUserDepartment(eCMUserPermissionIn.iduser, "004-006", "004-006", sEAdministration, events, eCMUserPermissionIn.userId, eCMUserPermissionIn.key, 1);
            }

            var message = sEAdministration.deleteUserDepartment(eCMUserPermissionIn.iduser, eCMUserPermissionIn.main, eCMUserPermissionIn.main);
            if (message > -1)
            {
                AddUserDepartment(eCMUserPermissionIn.iduser, eCMUserPermissionIn.main, eCMUserPermissionIn.main, sEAdministration, events, eCMUserPermissionIn.userId, eCMUserPermissionIn.key, 1);
            }

            if (string.IsNullOrEmpty(eCMUserPermissionIn.team))
            {
                sEGeneric.addUserToTeam("ACERVO ACADEMICO", eCMUserPermissionIn.iduser);
            }

            return true;
        }

        private static void AddUserDepartment(string iduser, string idarea, string idfunction, SEAdministration sEAdministration, Events events, string userId, string key, int exec)
        {
            try
            {
                var message = sEAdministration.addUserDepartment(iduser, idarea, idfunction);

                if (message == -1)
                {
                    throw new Exception(i18n.Resource.UnknownError);
                }
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(userId, key, "Erro", "SoftExpert.SEUserPermission.AddUserDepartment", string.Format("User: {0}. Area: {1}. Function{2}. Erro: {3}", iduser, idarea, idfunction, ex.Message));

                if (exec < 5)
                {
                    exec++;
                    int sleep = 3000;
                    int.TryParse(ConfigurationManager.AppSettings["SLEEP"], out sleep);

                    Thread.Sleep(sleep);
                    AddUserDepartment(iduser, idarea, idfunction, sEAdministration, events, userId, key, exec);
                }
            }
        }
    }
}
