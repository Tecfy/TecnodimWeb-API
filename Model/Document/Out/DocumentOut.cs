using Model.Common;
using Model.VM;

namespace Model.Out
{
    public class DocumentOut : ResultServiceVM
    {
        public DocumentOut()
        {
            this.Result = new DocumentVM();
        }

        public DocumentVM Result { get; set; }
    }
}
