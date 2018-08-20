using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class SEDocumentsOut : ResultServiceVM
    {
        public SEDocumentsOut()
        {
            this.result = new List<SEDocumentsVM>();
        }

        public List<SEDocumentsVM> result { get; set; }
    }
}
