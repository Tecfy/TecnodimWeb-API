using Model.VM;

namespace Model.Out
{
    public class ECMDocumentDetailByRegistrationOut : ResultServiceVM
    {
        public ECMDocumentDetailByRegistrationOut()
        {
            this.result = new ECMDocumentDetailByRegistrationVM();
        }

        public ECMDocumentDetailByRegistrationVM result { get; set; }
    }
}
