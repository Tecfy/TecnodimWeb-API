using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMDocumentDetailsByRegistrationOut : ResultServiceVM
    {
        public ECMDocumentDetailsByRegistrationOut()
        {
            this.result = new List<ECMDocumentDetailsByRegistrationVM>();
        }

        public List<ECMDocumentDetailsByRegistrationVM> result { get; set; }
    }
}
