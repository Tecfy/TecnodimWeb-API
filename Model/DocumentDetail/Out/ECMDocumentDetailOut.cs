using Model.VM;

namespace Model.Out
{
    public class ECMDocumentDetailOut : ResultServiceVM
    {
        public ECMDocumentDetailOut()
        {
            this.result = new ECMDocumentDetailVM();
        }

        public ECMDocumentDetailVM result { get; set; }
    }
}
