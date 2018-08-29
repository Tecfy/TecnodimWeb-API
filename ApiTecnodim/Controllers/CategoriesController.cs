using Model.In;
using Model.Out;
using Repository;
using System;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class CategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        CategoryRepository categoryRepository = new CategoryRepository();

        [Authorize, HttpGet]
        public ECMCategoriesOut GetECMCategories()
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMCategoriesIn ecmCategoriesIn = new ECMCategoriesIn() { userId = new Guid(User.Identity.Name), key = Key };

                ecmCategoriesOut = categoryRepository.GetECMCategories(ecmCategoriesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.CategoriesController.GetECMCategories", ex.Message);

                ecmCategoriesOut.successMessage = null;
                ecmCategoriesOut.messages.Add(ex.Message);
            }

            return ecmCategoriesOut;
        }
    }
}
