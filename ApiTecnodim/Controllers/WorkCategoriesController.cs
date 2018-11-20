using Model.In;
using Model.Out;
using Model.VM;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class WorkCategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        WorkCategoryRepository workCategoryRepository = new WorkCategoryRepository();

        [Authorize, HttpPost]
        public ECMWorkCategorySaveOut PostECMWorkCategorySave(ECMWorkCategorySaveIn ecmWorkCategorySaveIn)
        {
            ECMWorkCategorySaveOut ecmWorkCategorySaveOut = new ECMWorkCategorySaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ecmWorkCategorySaveIn.userId = User.Identity.Name;
                    ecmWorkCategorySaveIn.key = Key.ToString();

                    ecmWorkCategorySaveOut = workCategoryRepository.PostECMWorkCategorySave(ecmWorkCategorySaveIn);
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.WorkCategorysController.GetSEWorkCategorySave", ex.Message);

                ecmWorkCategorySaveOut.successMessage = null;
                ecmWorkCategorySaveOut.messages.Add(ex.Message);
            }

            return ecmWorkCategorySaveOut;
        }
    }
}
