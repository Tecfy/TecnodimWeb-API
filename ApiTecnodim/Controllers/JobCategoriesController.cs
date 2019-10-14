using Model.In;
using Model.Out;
using RegisterEvent.Events;
using Repository;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace ApiTecnodim.Controllers
{
    public class JobCategoriesController : ApiController
    {
        Events events = new Events();
        JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        [Authorize, HttpGet]
        public ECMJobCategoryOut GetECMJobCategory(string id)
        {
            ECMJobCategoryOut eCMJobCategoryOut = new ECMJobCategoryOut();
            Guid Key = Guid.NewGuid();

            try
            {
                ECMJobCategoryIn eCMJobCategoryIn = new ECMJobCategoryIn() { externalId = id, categoryId = WebConfigurationManager.AppSettings["SoftExpert.Category.JobCategory"], userId = User.Identity.Name, key = Key.ToString() };

                eCMJobCategoryOut = jobCategoryRepository.GetECMJobCategory(eCMJobCategoryIn);
            }
            catch (Exception ex)
            {
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.JobCategoriesController.GetECMJobCategory", ex.Message);

                eCMJobCategoryOut.successMessage = null;
                eCMJobCategoryOut.messages.Add(ex.Message);
            }

            return eCMJobCategoryOut;
        }

        [Authorize, HttpPost]
        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn eCMJobCategorySaveIn)
        {
            ECMJobCategorySaveOut eCMJobCategorySaveOut = new ECMJobCategorySaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    eCMJobCategorySaveIn.userId = User.Identity.Name;
                    eCMJobCategorySaveIn.key = Key.ToString();

                    eCMJobCategorySaveOut = jobCategoryRepository.SetECMJobCategorySave(eCMJobCategorySaveIn);
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
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.WorkCategorysController.SetECMJobCategorySave", ex.Message);

                eCMJobCategorySaveOut.successMessage = null;
                eCMJobCategorySaveOut.messages.Add(ex.Message);
            }

            return eCMJobCategorySaveOut;
        }

        [Authorize, HttpPost]
        public ECMJobSaveOut PostECMJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            ECMJobSaveOut eCMJobSaveOut = new ECMJobSaveOut();
            Guid Key = Guid.NewGuid();

            try
            {
                if (ModelState.IsValid)
                {
                    eCMJobSaveIn.userId = User.Identity.Name;
                    eCMJobSaveIn.key = Key.ToString();
                    eCMJobSaveIn.now = DateTime.Now;

                    eCMJobSaveOut = jobCategoryRepository.PostECMJobSave(eCMJobSaveIn);
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
                events.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.JobCategoriesController.PostECMJobSave", ex.Message);

                eCMJobSaveOut.successMessage = null;
                eCMJobSaveOut.messages.Add(ex.Message);
            }

            return eCMJobSaveOut;
        }
    }
}
