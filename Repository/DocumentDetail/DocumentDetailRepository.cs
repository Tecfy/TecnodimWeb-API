using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public partial class DocumentDetailRepository
    {
        Events events = new Events();

        public ECMDocumentDetailsByRegistrationOut GetECMDocumentDetailsByRegistration(ECMDocumentDetailsByRegistrationIn ecmDocumentDetailsByRegistrationIn)
        {
            ECMDocumentDetailsByRegistrationOut ecmDocumentDetailsByRegistrationOut = new ECMDocumentDetailsByRegistrationOut();

            events.SaveRegisterEvent(ecmDocumentDetailsByRegistrationIn.userId, ecmDocumentDetailsByRegistrationIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetailsByRegistration", "");

            ecmDocumentDetailsByRegistrationOut = SEDocumentDetail.GetSEDocumentDetailsByRegistration(ecmDocumentDetailsByRegistrationIn);

            if (ecmDocumentDetailsByRegistrationOut.result == null)
            {
                throw new System.Exception(i18n.Resource.StudentNotFound);
            }

            events.SaveRegisterEvent(ecmDocumentDetailsByRegistrationIn.userId, ecmDocumentDetailsByRegistrationIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetailsByRegistration", "");

            return ecmDocumentDetailsByRegistrationOut;
        }

        public ECMDocumentDetailByRegistrationOut GetECMDocumentDetailByRegistration(ECMDocumentDetailByRegistrationIn ecmDocumentDetailByRegistrationIn)
        {
            ECMDocumentDetailByRegistrationOut ecmDocumentDetailByRegistrationOut = new ECMDocumentDetailByRegistrationOut();

            events.SaveRegisterEvent(ecmDocumentDetailByRegistrationIn.userId, ecmDocumentDetailByRegistrationIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetailByRegistration", "");

            ecmDocumentDetailByRegistrationOut = SEDocumentDetail.GetSEDocumentDetailByRegistration(ecmDocumentDetailByRegistrationIn);

            if (ecmDocumentDetailByRegistrationOut.result == null)
            {
                throw new System.Exception(i18n.Resource.StudentNotFound);
            }

            events.SaveRegisterEvent(ecmDocumentDetailByRegistrationIn.userId, ecmDocumentDetailByRegistrationIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetailByRegistration", "");

            return ecmDocumentDetailByRegistrationOut;
        }

        public ECMDocumentDetailOut GetECMDocumentDetail(ECMDocumentDetailIn ecmDocumentDetailIn)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            events.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - Start", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");

            ecmDocumentDetailOut = SEDocumentDetail.GetSEDocumentDetail(ecmDocumentDetailIn);

            if (ecmDocumentDetailOut.result == null)
            {
                throw new System.Exception(i18n.Resource.StudentNotFound);
            }

            events.SaveRegisterEvent(ecmDocumentDetailIn.userId, ecmDocumentDetailIn.key, "Log - End", "Repository.DocumentDetailRepository.GetECMDocumentDetail", "");
            return ecmDocumentDetailOut;
        }
    }
}
