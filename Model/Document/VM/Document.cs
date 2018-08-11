using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Document.VM
{
    #region Helpers

    public class Document
    {
        public int documentId { get; set; }
        public string name { get; set; }
        public string registration { get; set; }
        public string archive { get; set; }        
    }

    #endregion
}
