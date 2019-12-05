using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMResendDocumentsOut : ResultServiceVM
    {
        public ECMResendDocumentsOut()
        {
            this.result = new List<ECMResendDocumentsVM>();
        }

        public List<ECMResendDocumentsVM> result { get; set; }
    }
}
