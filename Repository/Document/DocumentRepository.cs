using DataEF.DataAccess;
using Model.In;
using Model.Out;
using System.Linq;

namespace Repository
{
    public class DocumentRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public DocumentSaveOut SaveDocument(DocumentSaveIn documentSaveIn)
        {
            DocumentSaveOut documentSaveOut = new DocumentSaveOut();

            registerEventRepository.SaveRegisterEvent(documentSaveIn.userId.Value, documentSaveIn.key.Value, "Log - Start", "Repository.DocumentRepository.SaveDocument", "");

            using (var db = new DBContext())
            {
                Documents document = db.Documents.Where(x => x.ExternalId == documentSaveIn.documentId).FirstOrDefault();

                if (document == null)
                {
                    document = new Documents();

                    document.ExternalId = documentSaveIn.documentId;
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
