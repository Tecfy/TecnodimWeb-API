﻿using System;

namespace Model.In
{
    public class ECMJobCategorySaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string title { get; set; }

        public DateTime dataJob { get; set; }

        public string user { get; set; }

        public string extension { get; set; }

        public string DocumentId
        {
            get
            {
                return registration.Trim() + "-" + categoryId.Trim();
            }
        }

        public string FileName
        {
            get
            {
                return DocumentId + "-" + user + "-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
            }
        }

        public byte[] FileBinary
        {
            get
            {
                return Convert.FromBase64String(archive);
            }
        }
    }
}
