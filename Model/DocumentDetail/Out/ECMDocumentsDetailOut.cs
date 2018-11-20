using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMDocumentsDetailOut : ResultServiceVM
    {
        public ECMDocumentsDetailOut()
        {
            this.result = new List<ECMDocumentsDetailVM>();
        }

        public List<ECMDocumentsDetailVM> result { get; set; }
    }
}
