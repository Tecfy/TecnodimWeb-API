using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using SoftExpert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace Repository
{
    public class CategoryRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public ECMCategoriesOut GetECMCategories(ECMCategoriesIn ecmCategoriesIn)
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();
            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId, ecmCategoriesIn.key, "Log - Start", "Repository.CategoryRepository.GetECMCategories", "");

            ecmCategoriesOut = SECategory.GetSECategories();

            List<ECMCategoriesVM> ecmCategoriesVMs = new List<ECMCategoriesVM>();

            foreach (var item in ecmCategoriesOut.result)
            {
                if (ValidateCategory(item, ecmCategoriesOut.result))
                    ecmCategoriesVMs.Add(item);
            }

            ecmCategoriesOut.result = new List<ECMCategoriesVM>();
            ecmCategoriesOut.result = ecmCategoriesVMs;

            registerEventRepository.SaveRegisterEvent(ecmCategoriesIn.userId, ecmCategoriesIn.key, "Log - End", "Repository.CategoryRepository.GetECMCategories", "");
            return ecmCategoriesOut;
        }

        private bool ValidateCategory(ECMCategoriesVM item, List<ECMCategoriesVM> ecmCategoriesVMs)
        {
            bool response = false;

            ECMCategoriesVM ecmCategoriesVM = new ECMCategoriesVM();
            ecmCategoriesVM = ecmCategoriesVMs.Where(x => x.categoryId == item.parentId).FirstOrDefault();

            if ((ecmCategoriesVM != null && ecmCategoriesVM.code == WebConfigurationManager.AppSettings["Repository.CategoryRepository.ParentCategory"]) || (item.code == WebConfigurationManager.AppSettings["Repository.CategoryRepository.ParentCategory"]))
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
