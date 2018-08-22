using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using System.Linq;

namespace SoftExpert
{
    public static class SECategory
    {
        public static ECMCategoriesOut GetSECategories()
        {
            ECMCategoriesOut ecmCategoriesOut = new ECMCategoriesOut();

            SEClient seClient = SEConnection.GetConnection();
            searchCategoryReturn searchCategoryReturn = seClient.searchCategory();

            ecmCategoriesOut.result = searchCategoryReturn
                                    .RESULTARRAY
                                    .Select(x => new ECMCategoriesVM()
                                    {
                                        categoryId = x.CDCATEGORY,
                                        parentId = x.CDCATEGORYOWNER,
                                        code = x.IDCATEGORY,
                                        name = x.NMCATEGORY
                                    })
                                    .OrderBy(x => x.code)
                                    .ToList();


            return ecmCategoriesOut;
        }
    }
}
