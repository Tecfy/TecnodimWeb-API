using Model.VM;

namespace Model.Out
{
    public class SEDocumentDetailOut : ResultServiceVM
    {
        public SEDocumentDetailOut()
        {
            this.result = new SEDocumentDetailVM();
        }

        public SEDocumentDetailVM result { get; set; }
    }
}
