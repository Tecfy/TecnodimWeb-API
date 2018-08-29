using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMCategoriesOut GetECMCategories(ECMCategoriesIn ecmCategoriesIn)
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();
            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId.Value, ecmCategoriesIn.key.Value, "Log - Start", "Repository.CategoryRepository.GetECMCategories", "");

            ecmCategoriesOut = SECategory.GetSECategories();

            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId.Value, ecmCategoriesIn.key.Value, "Log - End", "Repository.CategoryRepository.GetECMCategories", "");
            return ecmCategoriesOut;
        }      
    }
}
