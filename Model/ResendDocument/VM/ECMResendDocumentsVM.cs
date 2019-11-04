using System.Collections.Generic;

namespace Model.VM
{
    public class ECMResendDocumentsVM
    {
        public string registration { get; set; }

        public string cpf { get; set; }

        public string name { get; set; }

        public List<ECMResendDocumentItemVM> itens { get; set; }
    }
}