using Model.VM;

namespace Model.Out
{
    public class SEDocumentOut : ResultServiceVM
    {
        public SEDocumentOut()
        {
            this.result = new SEDocumentVM();
        }

        public SEDocumentVM result { get; set; }
    }
}
