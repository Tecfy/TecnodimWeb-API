﻿using Model.In;
using Model.Out;
using Model.VM;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class DocumentsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentRepository documentRepository = new DocumentRepository();

        [Authorize, HttpGet]
        public ECMDocumentOut GetECMDocument(string id)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentIn ecmDocumentIn = new ECMDocumentIn() { externalId = id, userId = new Guid(User.Identity.Name), key = Key };

                ecmDocumentOut = documentRepository.GetECMDocument(ecmDocumentIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocument", ex.Message);

                ecmDocumentOut.successMessage = null;
                ecmDocumentOut.messages.Add(ex.Message);
            }

            return ecmDocumentOut;
        }

        [Authorize, HttpGet]
        public ECMDocumentsOut GetECMDocuments()
        {
            ECMDocumentsOut ecmDocumentsOut = new ECMDocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentsIn ecmDocumentsIn = new ECMDocumentsIn() { userId = new Guid(User.Identity.Name), key = Key };

                ecmDocumentsOut = documentRepository.GetECMDocuments(ecmDocumentsIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocuments", ex.Message);

                ecmDocumentsOut.successMessage = null;
                ecmDocumentsOut.messages.Add(ex.Message);
            }

            return ecmDocumentsOut;
        }

        [Authorize, HttpPost]
        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ecmDocumentSaveIn.userId = new Guid(User.Identity.Name);
                    ecmDocumentSaveIn.key = Key;

                    byte[] archive = System.IO.File.ReadAllBytes(@"C:\\Temp\\Tecnodim\\VICTOR - CONTRATOS.pdf");

                    ecmDocumentSaveIn.archive = Convert.FromBase64String(System.Convert.ToBase64String(archive));

                    ecmDocumentSaveOut = documentRepository.PostECMDocumentSave(ecmDocumentSaveIn);
                }
                else
                {
                    foreach (ModelState modelState in ModelState.Values)
                    {
                        var errors = modelState.Errors;
                        if (errors.Any())
                        {
                            foreach (ModelError error in errors)
                            {
                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", ex.Message);

                ecmDocumentSaveOut.successMessage = null;
                ecmDocumentSaveOut.messages.Add(ex.Message);
            }

            return ecmDocumentSaveOut;
        }
    }
}
