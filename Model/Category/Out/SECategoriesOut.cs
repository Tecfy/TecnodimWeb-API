using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SECategoriesOut : ResultServiceVM
    {
        public SECategoriesOut()
        {
            this.result = new List<SECategoriesVM>();
        }

        public List<SECategoriesVM> result { get; set; }
    }
}
