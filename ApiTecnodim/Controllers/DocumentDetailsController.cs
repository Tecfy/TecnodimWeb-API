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
        public DocumentDetailOut GetDocumentDetail(int documentId)
        {
            DocumentDetailOut studentOut = new DocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentDetailIn studentIn = new DocumentDetailIn() { documentId = documentId, userId = new Guid(User.Identity.Name), key = Key };

                    studentOut = studentRepository.GetDocumentDetail(studentIn);
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

                studentOut.result = null;
                studentOut.successMessage = null;
                studentOut.messages.Add(ex.Message);
            }

            return studentOut;
        }
    }
}
