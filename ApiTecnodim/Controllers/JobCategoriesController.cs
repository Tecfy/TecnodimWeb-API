using Model.In;
using Model.Out;
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
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();
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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.JobCategoriesController.GetECMJobCategory", ex.Message);

                eCMJobCategoryOut.successMessage = null;
                eCMJobCategoryOut.messages.Add(ex.Message);
            }

            return eCMJobCategoryOut;
        }

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
                    ecmWorkCategorySaveIn.now = DateTime.Now;

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
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.JobCategoriesController.PostECMJobSave", ex.Message);

                eCMJobSaveOut.successMessage = null;
                eCMJobSaveOut.messages.Add(ex.Message);
            }

            return eCMJobSaveOut;
        }

        [Authorize, HttpPost]
        public ECMJobDeletedOut DeleteECMJobArchive(ECMJobDeletedIn eCMJobDeletedIn)
        {
            ECMJobDeletedOut eCMJobDeletedOut = new ECMJobDeletedOut();
            Guid Key = Guid.NewGuid();

            try
            {
                jobCategoryRepository.DeleteECMJobArchive(eCMJobDeletedIn);
            }
            catch (Exception ex)
            {
                registerEventRepository.SaveRegisterEvent(User.Identity.Name, Key.ToString(), "Erro", "ApiTecnodim.Controllers.JobCategoriesController.DeleteECMJobArchive", ex.Message);

                eCMJobDeletedOut.successMessage = null;
                eCMJobDeletedOut.messages.Add(ex.Message);
            }

            return eCMJobDeletedOut;
        }
    }
}
