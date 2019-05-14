using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class JobCategoriesMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int JobCategoryId { get; set; } // JobCategoryId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Job", ResourceType = typeof(i18n.Resource))]
		public int JobId { get; set; } // JobId

		[Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
		public int CategoryId { get; set; } // CategoryId

		[StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public string Code { get; set; } // Code

		[Display(Name = "Received", ResourceType = typeof(i18n.Resource))]
		public bool Received { get; set; } // Received

		[Display(Name = "Send", ResourceType = typeof(i18n.Resource))]
		public bool Send { get; set; } // Send

		[Display(Name = "Sending", ResourceType = typeof(i18n.Resource))]
		public bool Sending { get; set; } // Sending

		[Display(Name = "SendingDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? SendingDate { get; set; } // SendingDate

        [Display(Name = "Hash", ResourceType = typeof(i18n.Resource))]
        public Guid Hash { get; set; } // Hash

        [Display(Name = "Sent", ResourceType = typeof(i18n.Resource))]
        public bool Sent { get; set; } // Sent

        [Display(Name = "Pages", ResourceType = typeof(i18n.Resource))]
        public int? Pages { get; set; } // Pages

        [Display(Name = "Download", ResourceType = typeof(i18n.Resource))]
        public bool Download { get; set; } // Download

        [Display(Name = "DownloadDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DownloadDate { get; set; } // DownloadDate
    }
}
