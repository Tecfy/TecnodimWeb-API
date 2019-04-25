using Model.In;
using Model.Out;
using Newtonsoft.Json;
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

        [Authorize, HttpGet]
        public ECMDocumentsValidateOut GetECMValidateDocuments()
        {
            ECMDocumentsValidateOut eCMDocumentsValidateOut = new ECMDocumentsValidateOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentsValidateIn eCMDocumentsValidateIn = new ECMDocumentsValidateIn() { userId = User.Identity.Name, key = Key.ToString() };

                eCMDocumentsValidateOut = documentRepository.GetECMValidateDocuments(eCMDocumentsValidateIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMValidateDocuments", ex.Message);

                eCMDocumentsValidateOut.successMessage = null;
                eCMDocumentsValidateOut.messages.Add(ex.Message);
            }

            return eCMDocumentsValidateOut;
        }

        [Authorize, HttpGet]
        public ECMDocumentsValidateAdInterfaceOut GetECMValidateAdInterfaceDocuments()
        {
            ECMDocumentsValidateAdInterfaceOut eCMDocumentsValidateAdInterfaceOut = new ECMDocumentsValidateAdInterfaceOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentsValidateAdInterfaceIn eCMDocumentsValidateAdInterfaceIn = new ECMDocumentsValidateAdInterfaceIn() { userId = User.Identity.Name, key = Key.ToString() };

                eCMDocumentsValidateAdInterfaceOut = documentRepository.GetECMValidateAdInterfaceDocuments(eCMDocumentsValidateAdInterfaceIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetECMValidateAdInterfaceDocuments", ex.Message);

                eCMDocumentsValidateAdInterfaceOut.successMessage = null;
                eCMDocumentsValidateAdInterfaceOut.messages.Add(ex.Message);
            }

            return eCMDocumentsValidateAdInterfaceOut;
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
                                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentsController.GetSEDocumentSave", JsonConvert.SerializeObject(error.ErrorMessage));

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
    }
}
