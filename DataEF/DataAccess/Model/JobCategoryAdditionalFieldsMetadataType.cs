﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class JobCategoryAdditionalFieldsMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int JobCategoryAdditionalFieldId { get; set; } // JobCategoryAdditionalFieldId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "JobCategory", ResourceType = typeof(i18n.Resource))]
		public int JobCategoryId { get; set; } // JobCategoryId

		[Display(Name = "CategoryAdditionalField", ResourceType = typeof(i18n.Resource))]
		public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Value", ResourceType = typeof(i18n.Resource))]
		public string Value { get; set; } // Value       
    }
}
