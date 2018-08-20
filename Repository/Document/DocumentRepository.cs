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

        public SEDocumentOut GetSEDocument(SEDocumentIn seDocumentIn)
        {
            SEDocumentOut seDocumentOut = new SEDocumentOut();
            registerEventRepository.SaveRegisterEvent(seDocumentIn.userId.Value, seDocumentIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetSEDocument", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.Active == true && x.DeletedDate == null && x.DocumentId == seDocumentIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    throw new System.Exception(i18n.Resource.RegisterNotFound);
                }

                seDocumentIn.externalId = document.ExternalId;
                seDocumentIn.hash = document.Hash.ToString();
            }

            seDocumentOut = Document.GetSEDocument(seDocumentIn);

            registerEventRepository.SaveRegisterEvent(seDocumentIn.userId.Value, seDocumentIn.key.Value, "Log - End", "Repository.DocumentRepository.GetSEDocument", "");
            return seDocumentOut;
        }

        public SEDocumentsOut GetSEDocuments(SEDocumentsIn seDocumentsIn)
        {
            SEDocumentsOut seDocumentsOut = new SEDocumentsOut();
            registerEventRepository.SaveRegisterEvent(seDocumentsIn.userId.Value, seDocumentsIn.key.Value, "Log - Start", "Repository.DocumentRepository.GetSEDocuments", "");

            seDocumentsOut = Document.GetSEDocuments();

            registerEventRepository.SaveRegisterEvent(seDocumentsIn.userId.Value, seDocumentsIn.key.Value, "Log - End", "Repository.DocumentRepository.GetSEDocuments", "");
            return seDocumentsOut;
        }

        public DocumentSaveOut SaveDocument(DocumentSaveIn documentSaveIn)
        {
            DocumentSaveOut documentSaveOut = new DocumentSaveOut();

            registerEventRepository.SaveRegisterEvent(documentSaveIn.userId.Value, documentSaveIn.key.Value, "Log - Start", "Repository.DocumentRepository.SaveDocument", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.ExternalId == documentSaveIn.externalId).FirstOrDefault();

                if (document == null)
                {
                    document = new Documents();

                    document.ExternalId = documentSaveIn.externalId;
                    document.DocumentStatusId = (int)Helper.Enum.EDocumentStatus.New;
                    document.Registration = documentSaveIn.registration;
                    document.Name = documentSaveIn.name;

                    db.Documents.Add(document);
                    db.SaveChanges();
                }
            }

            registerEventRepository.SaveRegisterEvent(documentSaveIn.userId.Value, documentSaveIn.key.Value, "Log - End", "Repository.DocumentRepository.SaveDocument", "");
            return documentSaveOut;
        }

    }
}
