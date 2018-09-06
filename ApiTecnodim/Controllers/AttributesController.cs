using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class AttributesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        AttributeRepository attributeRepository = new AttributeRepository();

        [Authorize, HttpPost]
        public ECMAttributeOut PostECMAttributeUpdate(ECMAttributeIn ecmAttributeIn)
        {
            ECMAttributeOut ecmAttributeOut = new ECMAttributeOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ecmAttributeIn.userId = User.Identity.Name;
                    ecmAttributeIn.key = Key.ToString();

                    attributeRepository.ECMAttributeUpdate(ecmAttributeIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.AttributesController.PostECMAttributeUpdate", ex.Message);

                ecmAttributeOut.successMessage = null;
                ecmAttributeOut.messages.Add(ex.Message);
            }

            return ecmAttributeOut;
        }
    }
}
