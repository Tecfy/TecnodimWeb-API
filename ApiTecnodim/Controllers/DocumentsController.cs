using Model.In;
using Model.Out;
using Newtonsoft.Json;
using RegisterEvent.Events;
using Repository;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class DocumentsController : ApiController
    {
        Events events = new Events();
        DocumentRepository documentRepository = new DocumentRepository();

        [Authorize, HttpGet]
        public ECMDocumentOut GetECMDocument(string id)
        {
            ECMDocumentOut ecmDocumentOut = new ECMDocumentOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentIn ecmDocumentIn = new ECMDocumentIn() { externalId = id, categoryId = WebConfigurationManager.AppSettings["SoftExpert.SearchAttributePendingCategory"], userId = User.Identity.Name, key = Key.ToString() };

                ecmDocumentOut = documentRepository.GetECMDocument(ecmDocumentIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocument", ex.Message);

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
                ECMDocumentsIn ecmDocumentsIn = new ECMDocumentsIn() { userId = User.Identity.Name, key = Key.ToString() };

                ecmDocumentsOut = documentRepository.GetECMDocuments(ecmDocumentsIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocuments", ex.Message);

                ecmDocumentsOut.successMessage = null;
                ecmDocumentsOut.messages.Add(ex.Message);
            }

            return ecmDocumentsOut;
        }

        [Authorize, HttpPost]
        public ECMDocumentSaveOut PostECMDocumentSave(ECMDocumentSaveIn ecmDocumentSaveIn)
        {
            events.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - Start", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", "");

            ECMDocumentSaveOut ecmDocumentSaveOut = new ECMDocumentSaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ecmDocumentSaveIn.userId = User.Identity.Name;
                    ecmDocumentSaveIn.key = Key.ToString();
                    ecmDocumentSaveIn.now = DateTime.Now;

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
                                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", JsonConvert.SerializeObject(error.ErrorMessage));

                                throw new Exception(error.ErrorMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", ex.Message);

                ecmDocumentSaveOut.successMessage = null;
                ecmDocumentSaveOut.messages.Add(ex.Message);
            }

            events.SaveRegisterEvent(ecmDocumentSaveIn.userId, ecmDocumentSaveIn.key, "Log - End", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", "");

            return ecmDocumentSaveOut;
        }

        [Authorize, HttpDelete]
        public ECMDocumentDeleteOut DeleteECMDocument(string id)
        {
            ECMDocumentDeleteOut eCMDocumentDeleteOut = new ECMDocumentDeleteOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentDeleteIn eCMDocumentDeleteIn = new ECMDocumentDeleteIn() { externalId = id, userId = User.Identity.Name, key = Key.ToString() };

                eCMDocumentDeleteOut = documentRepository.DeleteECMDocument(eCMDocumentDeleteIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.DeleteECMDocument", ex.Message);

                eCMDocumentDeleteOut.successMessage = null;
                eCMDocumentDeleteOut.messages.Add(ex.Message);
            }

            return eCMDocumentDeleteOut;
        }
    }
}
