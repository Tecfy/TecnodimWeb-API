using Model.In;
using Model.Out;
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
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocument", ex.Message);

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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMDocuments", ex.Message);

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
                    ecmDocumentSaveIn.userId = User.Identity.Name;
                    ecmDocumentSaveIn.key = Key.ToString();

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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", ex.Message);

                ecmDocumentSaveOut.successMessage = null;
                ecmDocumentSaveOut.messages.Add(ex.Message);
            }

            return ecmDocumentSaveOut;
        }

        [Authorize, HttpPost]
        public ECMDocumentDeletedOut DeleteECMDocumentArchive(ECMDocumentDeletedIn ecmDocumentDeletedIn)
        {
            ECMDocumentDeletedOut exmDocumentDeletedOut = new ECMDocumentDeletedOut();
            Guid Key = Guid.NewGuid();

            try
            {
                documentRepository.DeleteECMDocumentArchive(ecmDocumentDeletedIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.DeleteECMDocumentArchive", ex.Message);

                exmDocumentDeletedOut.successMessage = null;
                exmDocumentDeletedOut.messages.Add(ex.Message);
            }

            return exmDocumentDeletedOut;
        }
    }
}
