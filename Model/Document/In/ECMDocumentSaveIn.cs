﻿using System;
using System.Collections.Generic;

namespace Model.In
{
    public class ECMDocumentSaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public string user { get; set; }

        public string sliceUser { get; set; }

        public string sliceUserRegistration { get; set; }

        public string classificationUser { get; set; }

        public string classificationUserRegistration { get; set; }

        public string extension { get; set; }

        public string classificationDate { get; set; }

        public string sliceDate { get; set; }

        public DateTime now { get; set; }

        public string unityCode { get; set; }

        public string externalId { get; set; }

        public string DocumentId => registration.Trim() + "-" + unityCode.Trim() + "-" + categoryId.Trim();

        public string FileName => DocumentId + "-" + now.ToString("ddMMyyyy-HHmmss") + extension;

        public byte[] FileBinary => Convert.FromBase64String(archive);

        public List<ECMAdditionalFieldSaveIn> additionalFields { get; set; }
    }
}
