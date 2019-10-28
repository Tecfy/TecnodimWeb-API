using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public class UserPermissionRepository
    {
        Events events = new Events();

        public ECMUserPermissionOut PostECMUserPermission(ECMUserPermissionIn eCMUserPermissionIn)
        {
            ECMUserPermissionOut ecmAttributeOut = new ECMUserPermissionOut();
            events.SaveRegisterEvent(eCMUserPermissionIn.userId, eCMUserPermissionIn.key, "Log - Start", "Repository.UserPermissionRepository.PostECMUserPermission", "");

            SEUserPermission.SEUserPermissionUpdate(eCMUserPermissionIn);

            events.SaveRegisterEvent(eCMUserPermissionIn.userId, eCMUserPermissionIn.key, "Log - End", "Repository.UserPermissionRepository.PostECMUserPermission", "");
            return ecmAttributeOut;
        }
    }
}
