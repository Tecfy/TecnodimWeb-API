using Model.In;
using Model.Out;
using SoftExpert;

namespace Repository
{
    public class JobCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMJobCategorySaveOut SetECMJobCategorySave(ECMJobCategorySaveIn ecmJobCategorySaveIn)
        {
            ECMJobCategorySaveOut ecmJobCategorySaveOut = new ECMJobCategorySaveOut();

            registerEventRepository.SaveRegisterEvent(ecmJobCategorySaveIn.userId, ecmJobCategorySaveIn.key, "Log - Start", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");

            SEJobCategory.SEJobCategorySave(ecmJobCategorySaveIn);

            registerEventRepository.SaveRegisterEvent(ecmJobCategorySaveIn.userId, ecmJobCategorySaveIn.key, "Log - End", "Repository.JobCategoryRepository.SetECMJobCategorySave", "");
            return ecmJobCategorySaveOut;
        }
    }
}
