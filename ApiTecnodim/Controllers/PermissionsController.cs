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
    }
}
