using Model.In;
using Model.Out;
using Model.VM;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class DocumentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        [Authorize, HttpGet]
        public DocumentOut GetDocumentById(int id)
        {
            DocumentOut documentOut = new DocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                Document document = documents.Where(x => x.documentId == id).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                byte[] archive = System.IO.File.ReadAllBytes(document.archive);

                documentOut.result.archive = System.Convert.ToBase64String(archive);
                documentOut.result.hash = document.hash;

                return documentOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentOut.successMessage = null;
                documentOut.messages.Add(ex.Message);

                return documentOut;
            }
        }

        [Authorize, HttpGet]
        public DocumentSaveOut GetDocuments()
        {
            DocumentSaveOut documentSaveOut = new DocumentSaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                foreach (var item in documents)
                {
                    DocumentSaveIn documentSaveIn = new DocumentSaveIn() { documentId = item.documentId, userId = new Guid(User.Identity.Name), key = Key };

                    documentSaveOut = documentRepository.SaveDocument(documentSaveIn);
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentSaveOut.successMessage = null;
                documentSaveOut.messages.Add(i18n.Resource.UnknownError);
            }

            return documentSaveOut;
        }

        private List<Document> CreateDocuments()
        {
            List<Document> Documents = new List<Document>
            {
                new Document {documentId = 1, name = "Flavia do Nascimento Alves", registration = "01201855", archive = @"C:\\Temp\\Tecnodim\\VICTOR - CONTRATOS.pdf", hash = "71C75BDD-11BB-4CB9-9DAB-2A180C3B45AA" },
                new Document {documentId = 2, name = "Cicero Klebson dos Santos Silva", registration = "07020342", archive = @"C:\\Temp\\Tecnodim\\VICTOR - DOCUMENTOS DIVERSOS.pdf", hash = "8085C658-C84D-48C9-A2BA-93FF5C172928" },
                new Document {documentId = 3, name = "Alessandra Viana de Lima", registration = "01203422", archive = @"C:\\Temp\\Tecnodim\\VICTOR - DOCUMENTOS.pdf", hash = "5DF653DB-BE9A-4555-B9FA-BB7A90CEBF75" },
            };

            return Documents;
        }
    }
}
