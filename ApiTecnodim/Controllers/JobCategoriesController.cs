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
