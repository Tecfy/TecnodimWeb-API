using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public partial class PermissionRepository
    {
        Events events = new Events();

        public ECMPermissionsOut GetECMPermissions(ECMPermissionsIn ecmPermissionsIn)
        {
            ECMPermissionsOut ecmPermissionsOut = new ECMPermissionsOut();

            events.SaveRegisterEvent(ecmPermissionsIn.userId, ecmPermissionsIn.key, "Log - Start", "Repository.PermissionRepository.GetECMPermissions", "");

            ecmPermissionsOut = SEPermission.GetSEPermissions(ecmPermissionsIn);

            events.SaveRegisterEvent(ecmPermissionsIn.userId, ecmPermissionsIn.key, "Log - End", "Repository.PermissionRepository.GetECMPermissions", "");

            return ecmPermissionsOut;
        }

        public ECMPermissionOut GetECMPermission(ECMPermissionIn ecmPermissionIn)
        {
            ECMPermissionOut ecmPermissionOut = new ECMPermissionOut();

            events.SaveRegisterEvent(ecmPermissionIn.userId, ecmPermissionIn.key, "Log - Start", "Repository.PermissionRepository.GetECMPermission", "");

            ecmPermissionOut = SEPermission.GetSEPermission(ecmPermissionIn);

            events.SaveRegisterEvent(ecmPermissionIn.userId, ecmPermissionIn.key, "Log - End", "Repository.PermissionRepository.GetECMPermission", "");

            return ecmPermissionOut;
        }
    }
}
