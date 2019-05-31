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

            //if (ecmPermissionsOut.result == null)
            //{
            //    throw new System.Exception(i18n.Resource.StudentNotFound);
            //}

            registerEventRepository.SaveRegisterEvent(ecmPermissionsIn.userId, ecmPermissionsIn.key, "Log - End", "Repository.PermissionRepository.GetECMPermissions", "");

            return ecmPermissionsOut;
        }
    }
}
