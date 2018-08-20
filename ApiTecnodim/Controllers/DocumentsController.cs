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
        public SEDocumentOut GetSEDocument(int id)
        {
            SEDocumentOut seDocumentOut = new SEDocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                SEDocumentIn seDocumentIn = new SEDocumentIn() { documentId = id, userId = new Guid(User.Identity.Name), key = Key };

                seDocumentOut = documentRepository.GetSEDocument(seDocumentIn);

                return seDocumentOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.GetDocument", ex.Message);

                seDocumentOut.successMessage = null;
                seDocumentOut.messages.Add(ex.Message);

                return seDocumentOut;
            }
        }

        [Authorize, HttpGet]
        public SEDocumentsOut GetSEDocuments()
        {
            SEDocumentsOut seDocumentsOut = new SEDocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                SEDocumentsIn seDocumentsIn = new SEDocumentsIn() { userId = new Guid(User.Identity.Name), key = Key };

                seDocumentsOut = documentRepository.GetSEDocuments(seDocumentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocuments", ex.Message);

                seDocumentsOut.successMessage = null;
                seDocumentsOut.messages.Add(i18n.Resource.UnknownError);
            }

            return seDocumentsOut;
        }
    }
}
