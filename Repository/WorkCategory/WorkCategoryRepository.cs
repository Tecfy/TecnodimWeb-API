using Helper.ServerMap;
using Model.In;
using Model.Out;
using SoftExpert;
using System;
using System.Configuration;

namespace Repository
{
    public class WorkCategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMWorkCategorySaveOut PostECMWorkCategorySave(ECMWorkCategorySaveIn ecmWorkCategorySaveIn)
        {
            ECMWorkCategorySaveOut ecmWorkCategorySaveOut = new ECMWorkCategorySaveOut();

            registerEventRepository.SaveRegisterEvent(ecmWorkCategorySaveIn.userId, ecmWorkCategorySaveIn.key, "Log - Start", "Repository.WorkCategoryRepository.PostECMWorkCategorySave", "");

            SEWorkCategory.SEWorkCategorySave(ecmWorkCategorySaveIn);

            registerEventRepository.SaveRegisterEvent(ecmWorkCategorySaveIn.userId, ecmWorkCategorySaveIn.key, "Log - End", "Repository.WorkCategoryRepository.PostECMWorkCategorySave", "");
            return ecmWorkCategorySaveOut;
        }
    }
}
