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
        public SEDocumentDetailOut GetDocumentDetail(string registration)
        {
            SEDocumentDetailOut seDocumentDetailOut = new SEDocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    SEDocumentDetailIn seDocumentDetailIn = new SEDocumentDetailIn() { registration = registration, userId = new Guid(User.Identity.Name), key = Key };

                    seDocumentDetailOut = studentRepository.GetDocumentDetail(seDocumentDetailIn);
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
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetDocumentDetail", ex.Message);

                seDocumentDetailOut.result = null;
                seDocumentDetailOut.successMessage = null;
                seDocumentDetailOut.messages.Add(ex.Message);
            }

            return seDocumentDetailOut;
        }
    }
}
