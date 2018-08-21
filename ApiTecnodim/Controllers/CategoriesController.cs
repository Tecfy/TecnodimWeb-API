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

        [Authorize, HttpGet]
        public CategoryOut GetCategory(int categoryId)
        {
            CategoryOut categoryOut = new CategoryOut();
            Guid Key = Guid.NewGuid();

            try
            {
                CategoryIn categoryIn = new CategoryIn() { categoryId = categoryId, userId = new Guid(User.Identity.Name), key = Key };

                categoryOut = categoryRepository.GetCategory(categoryIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.CategoriesController.GetCategory", ex.Message);

                categoryOut.successMessage = null;
                categoryOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categoryOut;
        }

        [Authorize, HttpGet]
        public CategoriesOut GetCategories()
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                CategoriesIn categoriesIn = new CategoriesIn() { userId = new Guid(User.Identity.Name), key = Key };

                categoriesOut = categoryRepository.GetCategories(categoriesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.CategoriesController.GetCategories", ex.Message);

                categoriesOut.successMessage = null;
                categoriesOut.messages.Add(i18n.Resource.UnknownError);
            }

            return categoriesOut;
        }
    }
}
