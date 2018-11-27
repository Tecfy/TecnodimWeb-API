﻿using Model.In;
using Model.Out;
using Repository;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class JobCategoriesController : ApiController
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        [Authorize, HttpPost]
        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn ecmWorkCategorySaveIn)
        {
            ECMJobCategorySaveOut ecmWorkCategorySaveOut = new ECMJobCategorySaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    ecmWorkCategorySaveIn.userId = User.Identity.Name;
                    ecmWorkCategorySaveIn.key = Key.ToString();

                    ecmWorkCategorySaveOut = jobCategoryRepository.SetECMJobCategorySave(ecmWorkCategorySaveIn);
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
