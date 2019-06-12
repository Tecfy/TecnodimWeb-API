using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class StudentDocumentOut : ResultServiceVM
    {
        public StudentDocumentOut()
        {
            this.result = new List<StudentDocumentVM>();
        }

        public List<StudentDocumentVM> result { get; set; }
    }
}
