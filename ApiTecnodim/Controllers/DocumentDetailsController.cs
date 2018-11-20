using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class DocumentDetailsController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        DocumentDetailRepository documentDetailRepository = new DocumentDetailRepository();

        [Authorize, HttpGet]
        public ECMDocumentsDetailOut GetECMDocumentsDetail(string registration, string unity)
        {
            ECMDocumentsDetailOut ecmDocumentsDetailOut = new ECMDocumentsDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ECMDocumentsDetailIn ecmDocumentsDetailIn = new ECMDocumentsDetailIn() { registration = registration, unity = unity, userId = User.Identity.Name, key = Key.ToString() };

                    ecmDocumentsDetailOut = documentDetailRepository.GetECMDocumentsDetail(ecmDocumentsDetailIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentsDetail", ex.Message);

                ecmDocumentsDetailOut.result = null;
                ecmDocumentsDetailOut.successMessage = null;
                ecmDocumentsDetailOut.messages.Add(ex.Message);
            }

            return ecmDocumentsDetailOut;
        }

        [Authorize, HttpGet]
        public ECMDocumentDetailOut GetECMDocumentDetail(string id)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ECMDocumentDetailIn ecmDocumentDetailIn = new ECMDocumentDetailIn() { registration = id, userId = User.Identity.Name, key = Key.ToString() };

                    ecmDocumentDetailOut = documentDetailRepository.GetECMDocumentDetail(ecmDocumentDetailIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentDetail", ex.Message);

                ecmDocumentDetailOut.result = null;
                ecmDocumentDetailOut.successMessage = null;
                ecmDocumentDetailOut.messages.Add(ex.Message);
            }

            return ecmDocumentDetailOut;
        }
    }
}
