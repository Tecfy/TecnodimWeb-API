using Model.In;
using Model.Out;
using Model.VM;
using RegisterEvent.Events;
using SoftExpert;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public class CategoryRepository
    {
        Events events = new Events();

        public ECMCategoriesOut GetECMCategories(ECMCategoriesIn ecmCategoriesIn)
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();
            events.SaveRegisterEvent(ecmCategoriesIn.userId, ecmCategoriesIn.key, "Log - Start", "Repository.CategoryRepository.GetECMCategories", "");

            ecmCategoriesOut = SECategory.GetSECategories();

            List<ECMCategoriesVM> ecmCategoriesVMs = new List<ECMCategoriesVM>();

            foreach (var item in ecmCategoriesOut.result)
            {
                if (ValidateCategory(item, ecmCategoriesOut.result))
                    ecmCategoriesVMs.Add(item);
            }

            ecmCategoriesOut.result = new List<ECMCategoriesVM>();
            ecmCategoriesOut.result = ecmCategoriesVMs;

            events.SaveRegisterEvent(ecmCategoriesIn.userId, ecmCategoriesIn.key, "Log - End", "Repository.CategoryRepository.GetECMCategories", "");
            return ecmCategoriesOut;
        }

        private bool ValidateCategory(ECMCategoriesVM item, List<ECMCategoriesVM> ecmCategoriesVMs)
        {
            bool response = false;

            ECMCategoriesVM ecmCategoriesVM = new ECMCategoriesVM();
            ecmCategoriesVM = ecmCategoriesVMs.Where(x => x.categoryId == item.parentId).FirstOrDefault();

            if ((ecmCategoriesVM != null && ecmCategoriesVM.code == WebConfigurationManager.AppSettings["SoftExpert.ParentCategory"]) 
                || (item.code == WebConfigurationManager.AppSettings["SoftExpert.ParentCategory"]))
            {
                response = true;
            }
            else if (ecmCategoriesVM != null && ecmCategoriesVM.parentId > 0)
            {
                response = ValidateCategory(ecmCategoriesVM, ecmCategoriesVMs);
            }

            return response;
        }
    }
}
