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
        public SECategoriesOut GetSECategories()
        {
            SECategoriesOut seCategoriesOut = new SECategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                SECategoriesIn seCategoriesIn = new SECategoriesIn() { userId = new Guid(User.Identity.Name), key = Key };

                seCategoriesOut = categoryRepository.GetSECategories(seCategoriesIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.CategoriesController.Get", ex.Message);

                seCategoriesOut.successMessage = null;
                seCategoriesOut.messages.Add(i18n.Resource.UnknownError);
            }

            return seCategoriesOut;
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
