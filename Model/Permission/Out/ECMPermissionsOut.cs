using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class ECMPermissionsOut : ResultServiceVM
    {
        public ECMPermissionsOut()
        {
            this.result = new List<ECMPermissionsVM>();
        }

        public List<ECMPermissionsVM> result { get; set; }
    }
}
