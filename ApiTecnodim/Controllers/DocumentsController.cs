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

        [Authorize, HttpGet]
        public DocumentOut Get(int documentId)
        {
            DocumentOut documentOut = new DocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                byte[] archive = System.IO.File.ReadAllBytes(documents.Where(x => x.documentId == documentId).FirstOrDefault().archive);

                documentOut.result.archive = System.Convert.ToBase64String(archive);

                return documentOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentOut.successMessage = null;
                documentOut.messages.Add(i18n.Resource.UnknownError);

                return documentOut;
            }
        }

        [Authorize, HttpGet]
        public DocumentsOut GetDocuments(int documentId)
        {
            DocumentsOut documentsOut = new DocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                documentsOut.result = documents.Select(x => new DocumentsVM() { documentId = x.documentId, name = x.name, registration = x.registration }).ToList();


            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.Get", ex.Message);

                documentsOut.successMessage = null;
                documentsOut.messages.Add(i18n.Resource.UnknownError);
            }

            return documentsOut;
        }

        private List<Document> CreateDocuments()
        {
            List<Document> Documents = new List<Document>
            {
                new Document {documentId = 1, name = "Alessandra Viana de Lima", registration = "01203422", archive = @"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - DOCUMENTOS.pdf" },
                new Document {documentId = 2, name = "Cicero Klebson dos Santos Silva", registration = "07020342", archive = @"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - DOCUMENTOS DIVERSOS.pdf"},
                new Document {documentId = 3, name = "Flavia do Nascimento Alves", registration = "01201855", archive = @"D:\\Rudolf\\Tecfy\\Tecnodim\\VICTOR - CONTRATOS.pdf" },
            };

            return Documents;
        }
    }
}
