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
        DocumentDetailRepository studentRepository = new DocumentDetailRepository();

        [Authorize, HttpGet]
        public ECMDocumentDetailOut GetECMDocumentDetail(string id)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ECMDocumentDetailIn ecmDocumentDetailIn = new ECMDocumentDetailIn() { registration = id, userId = new Guid(User.Identity.Name), key = Key };

                    ecmDocumentDetailOut = studentRepository.GetECMDocumentDetail(ecmDocumentDetailIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentDetail", ex.Message);

                ecmDocumentDetailOut.result = null;
                ecmDocumentDetailOut.successMessage = null;
                ecmDocumentDetailOut.messages.Add(ex.Message);
            }

            return ecmDocumentDetailOut;
        }
    }
}
