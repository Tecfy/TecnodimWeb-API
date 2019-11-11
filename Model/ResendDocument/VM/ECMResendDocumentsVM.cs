using System.Collections.Generic;

namespace Model.VM
{
    public class ECMResendDocumentsVM
    {
        public string id { get; set; }

        public string registration { get; set; }

        public string cpf { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public List<ECMResendDocumentItemVM> itens { get; set; }
    }
}