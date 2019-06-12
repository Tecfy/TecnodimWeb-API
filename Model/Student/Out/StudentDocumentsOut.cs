using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class StudentDocumentsOut : ResultServiceVM
    {
        public StudentDocumentsOut()
        {
            this.result = new List<StudentDocumentsVM>();
        }

        public List<StudentDocumentsVM> result { get; set; }
    }
}
