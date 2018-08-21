using Model.VM;

namespace Model.Out
{
    public class ECMDocumentOut : ResultServiceVM
    {
        public ECMDocumentOut()
        {
            this.result = new ECMDocumentVM();
        }

        public ECMDocumentVM result { get; set; }
    }
}
