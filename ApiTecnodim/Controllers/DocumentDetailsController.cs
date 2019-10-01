using Model.In;
using Model.Out;
using RegisterEvent.Events;
using Repository;
using System;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class DocumentDetailsController : ApiController
    {
        Events events = new Events();
        DocumentDetailRepository documentDetailRepository = new DocumentDetailRepository();

        [Authorize, HttpGet]
        public ECMDocumentDetailsByRegistrationOut GetECMDocumentDetailsByRegistration(string registration, string unity)
        {
            ECMDocumentDetailsByRegistrationOut ecmDocumentDetailsByRegistrationOut = new ECMDocumentDetailsByRegistrationOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentDetailsByRegistrationIn ecmDocumentDetailsByRegistrationIn = new ECMDocumentDetailsByRegistrationIn() { registration = registration, unity = unity, userId = User.Identity.Name, key = Key.ToString() };

                ecmDocumentDetailsByRegistrationOut = documentDetailRepository.GetECMDocumentDetailsByRegistration(ecmDocumentDetailsByRegistrationIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentDetailsByRegistration", ex.Message);

                ecmDocumentDetailsByRegistrationOut.result = null;
                ecmDocumentDetailsByRegistrationOut.successMessage = null;
                ecmDocumentDetailsByRegistrationOut.messages.Add(ex.Message);
            }

            return ecmDocumentDetailsByRegistrationOut;
        }

        [Authorize, HttpGet]
        public ECMDocumentDetailByRegistrationOut GetECMDocumentDetailByRegistration(string registration, string unity)
        {
            ECMDocumentDetailByRegistrationOut ecmDocumentDetailByRegistrationOut = new ECMDocumentDetailByRegistrationOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentDetailByRegistrationIn ecmDocumentDetailByRegistrationIn = new ECMDocumentDetailByRegistrationIn() { registration = registration, unity = unity, userId = User.Identity.Name, key = Key.ToString() };

                ecmDocumentDetailByRegistrationOut = documentDetailRepository.GetECMDocumentDetailByRegistration(ecmDocumentDetailByRegistrationIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentDetailByRegistration", ex.Message);

                ecmDocumentDetailByRegistrationOut.result = null;
                ecmDocumentDetailByRegistrationOut.successMessage = null;
                ecmDocumentDetailByRegistrationOut.messages.Add(ex.Message);
            }

            return ecmDocumentDetailByRegistrationOut;
        }

        [Authorize, HttpGet]
        public ECMDocumentDetailOut GetECMDocumentDetail(string id)
        {
            ECMDocumentDetailOut ecmDocumentDetailOut = new ECMDocumentDetailOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMDocumentDetailIn ecmDocumentDetailIn = new ECMDocumentDetailIn() { registration = id, userId = User.Identity.Name, key = Key.ToString() };

                ecmDocumentDetailOut = documentDetailRepository.GetECMDocumentDetail(ecmDocumentDetailIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.DocumentDetailsController.GetECMDocumentDetail", ex.Message);

                ecmDocumentDetailOut.result = null;
                ecmDocumentDetailOut.successMessage = null;
                ecmDocumentDetailOut.messages.Add(ex.Message);
            }

            return ecmDocumentDetailOut;
        }     
    }
}
