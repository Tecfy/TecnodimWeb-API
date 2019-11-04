using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public partial class ResendDocumentRepository
    {
        Events events = new Events();

        public ECMResendDocumentsOut GetECMResendDocuments(ECMResendDocumentsIn eCMResendDocumentsIn)
        {
            ECMResendDocumentsOut eCMResendDocumentsOut = new ECMResendDocumentsOut();

            events.SaveRegisterEvent(eCMResendDocumentsIn.userId, eCMResendDocumentsIn.key, "Log - Start", "Repository.ResendDocumentRepository.GetECMResendDocuments", "");

            eCMResendDocumentsOut = SEResendDocument.GetECMResendDocuments(eCMResendDocumentsIn);

            if (eCMResendDocumentsOut.result == null)
            {
                throw new System.Exception(i18n.Resource.StudentNotFound);
            }

            events.SaveRegisterEvent(eCMResendDocumentsIn.userId, eCMResendDocumentsIn.key, "Log - End", "Repository.ResendDocumentRepository.GetECMResendDocuments", "");

            return eCMResendDocumentsOut;
        }       
    }
}
