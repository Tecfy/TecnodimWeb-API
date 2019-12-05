using System;
using System.Collections.Generic;

namespace Model.In
{
    public class ECMJobSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public string user { get; set; }

        public string extension { get; set; }

        public DateTime now { get; set; }

        public string unityCode { get; set; }

        public string unityName { get; set; }

        public string DocumentId
        {
            get
            {
                return registration.Trim() + "-" + unityCode.Trim() + "-" + categoryId.Trim();
            }
        }

        public string FileName
        {
            get
            {
                return DocumentId + "-" + now.ToString("ddMMyyyy-HHmmss") + extension;
            }
        }

        public byte[] FileBinary
        {
            get
            {
                return Convert.FromBase64String(archive);
            }
        }

        public List<ECMAdditionalFieldSaveIn> additionalFields { get; set; }
    }
}
