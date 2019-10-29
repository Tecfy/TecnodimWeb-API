using Model.In;
using Model.Out;
using RegisterEvent.Events;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class UserPermissionsController : ApiController
    {
        Events events = new Events();
        UserPermissionRepository userPermissionRepository = new UserPermissionRepository();

        [Authorize, HttpPost]
        public ECMUserPermissionOut PostECMUserPermission(ECMUserPermissionIn eCMUserPermissionIn)
        {
            ECMUserPermissionOut eCMUserPermissionOut = new ECMUserPermissionOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    eCMUserPermissionIn.userId = User.Identity.Name;
                    eCMUserPermissionIn.key = Key.ToString();

                    userPermissionRepository.PostECMUserPermission(eCMUserPermissionIn);
                }
                else
                {
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        var errors = modelState.Errors;
                        if (errors.Any())
                        {
                            foreach (ModelError error in errors)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.UserPermissionsController.PostECMUserPermission", ex.Message);

                eCMUserPermissionOut.successMessage = null;
                eCMUserPermissionOut.messages.Add(ex.Message);
            }

            return eCMUserPermissionOut;
        }
    }
}
