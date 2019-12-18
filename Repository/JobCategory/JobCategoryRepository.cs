using Model.In;
using Model.Out;
using RegisterEvent.Events;
using SoftExpert;

namespace Repository
{
    public class JobCategoryRepository
    {
        Events events = new Events();        

        public ECMJobSaveOut PostECMJobSave(ECMJobSaveIn eCMJobSaveIn)
        {
            ECMJobSaveOut ecmJobSaveOut = new ECMJobSaveOut();

            events.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - Start", "Repository.JobRepository.PostECMJobSave", "");

            SEJobCategory.SEDocumentSave(eCMJobSaveIn);

            events.SaveRegisterEvent(eCMJobSaveIn.userId, eCMJobSaveIn.key, "Log - End", "Repository.JobRepository.PostECMJobSave", "");
            return ecmJobSaveOut;
        }
    }
}
