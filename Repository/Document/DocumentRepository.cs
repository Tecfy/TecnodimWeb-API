using DataEF.DataAccess;
using Model.In;
using Model.Out;
using SoftExpert;
using System.Linq;

namespace Repository
{
    public class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMDocumentOut GetECMDocument(ECMDocumentIn seDocumentIn)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            registerEventRepository.SaveRegisterEvent(seDocumentIn.userId, seDocumentIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocument", "");

            ecmDocumentOut = SEDocument.GetSEDocument(seDocumentIn);

            registerEventRepository.SaveRegisterEvent(seDocumentIn.userId, seDocumentIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocument", "");
            return ecmDocumentOut;
        }

        public ECMDocumentsOut GetECMDocuments(ECMDocumentsIn ecmDocumentsIn)
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - Start", "Repository.DocumentRepository.GetECMDocuments", "");

            ecmDocumentsOut = SEDocument.GetSEDocuments();

            registerEventRepository.SaveRegisterEvent(ecmDocumentsIn.userId, ecmDocumentsIn.key, "Log - End", "Repository.DocumentRepository.GetECMDocuments", "");
            return ecmDocumentsOut;
        }

        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - Start", "Repository.DocumentRepository.PostECMDocumentSave", "");

            SEDocument.SEDocumentSave(ecmDocumentSaveIn);

            registerEventRepository.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - End", "Repository.DocumentRepository.PostECMDocumentSave", "");
            return ecmDocumentSaveOut;
        }
    }
}
