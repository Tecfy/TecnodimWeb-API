using Model.In;
using Model.Out;
using RegisterEvent.Events;
using Repository;
using System;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class ResendDocumentsController : ApiController
    {
        Events events = new Events();
        ResendDocumentRepository resendDocumentRepository = new ResendDocumentRepository();

        [Authorize, HttpGet]
        public ECMResendDocumentsOut GetECMResendDocuments(string registration, string unity)
        {
            ECMResendDocumentsOut eCMResendDocumentsOut = new ECMResendDocumentsOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMResendDocumentsIn eCMResendDocumentsIn = new ECMResendDocumentsIn() { registration = registration, unity = unity, userId = User.Identity.Name, key = Key.ToString() };

                eCMResendDocumentsOut = resendDocumentRepository.GetECMResendDocuments(eCMResendDocumentsIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.ResendDocumentsController.GetECMResendDocuments", ex.Message);

                eCMResendDocumentsOut.result = null;
                eCMResendDocumentsOut.successMessage = null;
                eCMResendDocumentsOut.messages.Add(ex.Message);
            }

            return eCMResendDocumentsOut;
        }
    }
}
