﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ECMAttributeIn : BaseIn
    {
        [Required]
        [Display(Name = "External", ResourceType = typeof(i18n.Resource))]
        public string externalId { get; set; }

        public List<ECMAttributeItemIn> itens { get; set; }
    }

    public class ECMAttributeItemIn
    {
        [Required]
        [Display(Name = "Attribute", ResourceType = typeof(i18n.Resource))]
        public string attribute { get; set; }

        [Required]
        [Display(Name = "Value", ResourceType = typeof(i18n.Resource))]
        public string value { get; set; }
    }
}
