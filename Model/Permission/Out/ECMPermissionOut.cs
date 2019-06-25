using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMPermissionOut : ResultServiceVM
    {
        public ECMPermissionOut()
        {
            this.result = new List<ECMPermissionVM>();
        }

        public List<ECMPermissionVM> result { get; set; }
    }
}
