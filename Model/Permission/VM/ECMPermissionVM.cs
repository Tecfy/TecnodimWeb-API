﻿namespace Model.VM
{
    public class ECMPermissionVM
    {
        public string externalId { get; set; }

        public bool cappservice { get; set; }

        public bool classify { get; set; }

        public bool slice { get; set; }

        public bool digitalizaMPF { get; set; }

        public bool resend { get; set; }

        public string name { get; set; }

        public string registration { get; set; }

        public string group { get; set; }

        public string status { get; set; }        
    }
}