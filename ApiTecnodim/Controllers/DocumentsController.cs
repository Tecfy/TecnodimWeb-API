﻿using Model.Out;
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
        public DocumentOut GetDocument(int externalId)
        {
            DocumentOut documentOut = new DocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                Document document = documents.Where(x => x.externalId == externalId).FirstOrDefault();

                if (document == null)
                {
                    throw new Exception(i18n.Resource.RegisterNotFound);
                }

                byte[] archive = System.IO.File.ReadAllBytes(document.archive);

                documentOut.result.archive = System.Convert.ToBase64String(archive);

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
        public DocumentsOut GetDocuments()
        {
            DocumentsOut documentsOut = new DocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Document> documents = new List<Document>();
                documents = CreateDocuments();

                documentsOut.result = documents.Select(x => new DocumentsVM() { externalId = x.externalId, name = x.name, registration = x.registration }).ToList();
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
                new Document {externalId = 1, name = "Flavia do Nascimento Alves", registration = "01201855", archive = @"C:\\Temp\\Tecnodim\\VICTOR - CONTRATOS.pdf" },
                new Document {externalId = 2, name = "Cicero Klebson dos Santos Silva", registration = "07020342", archive = @"C:\\Temp\\Tecnodim\\VICTOR - DOCUMENTOS DIVERSOS.pdf"},
                new Document {externalId = 3, name = "Alessandra Viana de Lima", registration = "01203422", archive = @"C:\\Temp\\Tecnodim\\VICTOR - DOCUMENTOS.pdf" },
            };

            return Documents;
        }
    }
}
