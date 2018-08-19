using Model.Out;
using Model.VM;
using SoftExpert.com.softexpert.tecfy;
using SoftExpert.Connection;
using System.Linq;

namespace SoftExpert.Category
{
    public static class Category
    {
        public static SECategoriesOut GetCategories()
        {
            SECategoriesOut seCategoriesOut = new SECategoriesOut();

            SEClient seClient = Connection.Connection.GetConnection();
            searchCategoryReturn searchCategoryReturn = seClient.searchCategory();

            seCategoriesOut.result = searchCategoryReturn
                                    .RESULTARRAY
                                    .Select(x => new SECategoriesVM()
                                    {
                                        code = x.IDCATEGORY.ToString(),
                                        name = x.NMCATEGORY
                                    })
                                    .OrderBy(x => x.code)
                                    .ToList();


            return seCategoriesOut;
        }
    }
}
