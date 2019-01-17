using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMDocumentDetailsByRegistrationOut GetECMDocumentDetailsByRegistration(ECMDocumentDetailsByRegistrationIn ecmDocumentDetailsByRegistrationIn)
        {
            ECMDocumentDetailsByRegistrationOut ecmDocumentDetailsByRegistrationOut = new ECMDocumentDetailsByRegistrationOut();

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailsByRegistrationIn.userId, ecmDocumentDetailsByRegistrationIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetailsByRegistration", "");

            ecmDocumentDetailsByRegistrationOut = SEDocumentDetail.GetSEDocumentDetailsByRegistration(ecmDocumentDetailsByRegistrationIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailsByRegistrationIn.userId, ecmDocumentDetailsByRegistrationIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetailsByRegistration", "");

            return ecmDocumentDetailsByRegistrationOut;
        }

        public ECMDocumentDetailByRegistrationOut GetECMDocumentDetailByRegistration(ECMDocumentDetailByRegistrationIn ecmDocumentDetailByRegistrationIn)
        {
            ECMDocumentDetailByRegistrationOut ecmDocumentDetailByRegistrationOut = new ECMDocumentDetailByRegistrationOut();

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailByRegistrationIn.userId, ecmDocumentDetailByRegistrationIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetailByRegistration", "");

            ecmDocumentDetailByRegistrationOut = SEDocumentDetail.GetSEDocumentDetailByRegistration(ecmDocumentDetailByRegistrationIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailByRegistrationIn.userId, ecmDocumentDetailByRegistrationIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetailByRegistration", "");

            return ecmDocumentDetailByRegistrationOut;
        }

        public ECMDocumentDetailOut GetECMDocumentDetail(ECMDocumentDetailIn ecmDocumentDetailIn)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");

            ecmDocumentDetailOut = SEDocumentDetail.GetSEDocumentDetail(ecmDocumentDetailIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");
            return ecmDocumentDetailOut;
        }

        public ECMDocumentDetailSaveOut PostECMDocumentDetailSave(ECMDocumentDetailSaveIn eCMDocumentDetailSaveIn)
        {
            ECMDocumentDetailSaveOut eCMDocumentDetailSaveOut = new ECMDocumentDetailSaveOut();

            registerEventRepository.SaveRegisterEvent(eCMDocumentDetailSaveIn.userId, eCMDocumentDetailSaveIn.key, "Log - Start", "Repository.DocumentDetailRepository.PostECMDocumentDetailSave", "");

            eCMDocumentDetailSaveOut.result.registration = SEDocumentDetail.SEDocumentDetailSave(eCMDocumentDetailSaveIn);

            registerEventRepository.SaveRegisterEvent(eCMDocumentDetailSaveIn.userId, eCMDocumentDetailSaveIn.key, "Log - End", "Repository.DocumentDetailRepository.PostECMDocumentDetailSave", "");
            return eCMDocumentDetailSaveOut;
        }
    }
}
