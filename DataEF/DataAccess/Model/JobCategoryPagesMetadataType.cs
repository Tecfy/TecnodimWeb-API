using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class JobCategoryPagesMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int JobCategoryPageId { get; set; } // JobCategoryPageId (Primary key)

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

		[Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
		public int Page { get; set; } // Page
    }
}
