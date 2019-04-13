using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMDocumentsValidateOut : ResultServiceVM
    {
        public ECMDocumentsValidateOut()
        {
            this.result = new List<ECMDocumentsValidateVM>();
        }

        public List<ECMDocumentsValidateVM> result { get; set; }
    }
}
