using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public partial class PermissionRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMPermissionsOut GetECMPermissions(ECMPermissionsIn ecmPermissionsIn)
        {
            ECMPermissionsOut ecmPermissionsOut = new ECMPermissionsOut();

            registerEventRepository.SaveRegisterEvent(ecmPermissionsIn.userId, ecmPermissionsIn.key, "Log - Start", "Repository.PermissionRepository.GetECMPermissions", "");

            ecmPermissionsOut = SEPermission.GetSEPermissions(ecmPermissionsIn);

            registerEventRepository.SaveRegisterEvent(ecmPermissionsIn.userId, ecmPermissionsIn.key, "Log - End", "Repository.PermissionRepository.GetECMPermissions", "");

            return ecmPermissionsOut;
        }

        public ECMPermissionOut GetECMPermission(ECMPermissionIn ecmPermissionIn)
        {
            ECMPermissionOut ecmPermissionOut = new ECMPermissionOut();

            registerEventRepository.SaveRegisterEvent(ecmPermissionIn.userId, ecmPermissionIn.key, "Log - Start", "Repository.PermissionRepository.GetECMPermission", "");

            ecmPermissionOut = SEPermission.GetSEPermission(ecmPermissionIn);

            registerEventRepository.SaveRegisterEvent(ecmPermissionIn.userId, ecmPermissionIn.key, "Log - End", "Repository.PermissionRepository.GetECMPermission", "");

            return ecmPermissionOut;
        }
    }
}
