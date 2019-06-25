using Model.In;
using Model.Out;
using Repository;
using System;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class PermissionsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        PermissionRepository permissionRepository = new PermissionRepository();

        [Authorize, HttpGet]
        public ECMPermissionsOut GetECMPermissions()
        {
            ECMPermissionsOut ecmPermissionsOut = new ECMPermissionsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMPermissionsIn ecmPermissionsIn = new ECMPermissionsIn() { userId = User.Identity.Name, key = Key.ToString() };

                ecmPermissionsOut = permissionRepository.GetECMPermissions(ecmPermissionsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.PermissionsController.GetECMPermissions", ex.Message);

                ecmPermissionsOut.result = null;
                ecmPermissionsOut.successMessage = null;
                ecmPermissionsOut.messages.Add(ex.Message);
            }

            return ecmPermissionsOut;
        }

        [Authorize, HttpGet]
        public ECMPermissionOut GetECMPermission(string id)
        {
            ECMPermissionOut ecmPermissionOut = new ECMPermissionOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMPermissionIn ecmPermissionIn = new ECMPermissionIn() { Registration = id, userId = User.Identity.Name, key = Key.ToString() };

                ecmPermissionOut = permissionRepository.GetECMPermission(ecmPermissionIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.PermissionsController.GetECMPermissions", ex.Message);

                ecmPermissionOut.result = null;
                ecmPermissionOut.successMessage = null;
                ecmPermissionOut.messages.Add(ex.Message);
            }

            return ecmPermissionOut;
        }
    }
}
