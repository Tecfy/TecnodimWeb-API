﻿using System;

namespace Model.VM
{
    public class SEDocumentsVM
    {
        public int documentStatusId { get; set; }

        public string externalId { get; set; }

        public string registration { get; set; }

        public string name { get; set; }

        public Guid hash { get; set; }
    }
}
