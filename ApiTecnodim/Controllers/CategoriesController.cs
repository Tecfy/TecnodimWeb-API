using Model.Out;
using Model.VM;
using Repository.RegisterEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiTecnodim.Controllers
{
    public class CategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        [Authorize, HttpGet]
        public CategoriesOut GetCategories()
        {
            CategoriesOut categoriesOut = new CategoriesOut();
            Guid Key = Guid.NewGuid();

            try
            {
                List<Category> categories = new List<Category>();
                categories = CreateCategories();

                categoriesOut.result = categories.Select(x => new CategoriesVM() { categoryId = x.categoryId, name = x.name }).ToList();

                return categoriesOut;
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(new Guid(User.Identity.Name), Key, "Erro", "ApiTecnodim.Controllers.CategoriesController.GetCategories", ex.Message);

                categoriesOut.successMessage = null;
                categoriesOut.messages.Add(i18n.Resource.UnknownError);

                return categoriesOut;
            }
        }

        private List<Category> CreateCategories()
        {
            List<Category> categories = new List<Category>
            {
                new Category {categoryId = 1, name = "Cursos de Graduação" },
                new Category {categoryId = 2, name = "Vida acadêmica dos alunos" },
                new Category {categoryId = 3, name = "Documentação acadêmica" },
            };

            return categories;
        }
    }
}
