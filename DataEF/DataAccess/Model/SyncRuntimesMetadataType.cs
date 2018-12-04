using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class SyncRuntimesMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int SyncRuntimeId { get; set; } // SyncRuntimeId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "URL", ResourceType = typeof(i18n.Resource))]
		public string Url { get; set; } // URL

		[Display(Name = "Interval", ResourceType = typeof(i18n.Resource))]
		public int Interval { get; set; } // Interval

		[Display(Name = "LastExecution", ResourceType = typeof(i18n.Resource))]
		public DateTime LastExecution { get; set; } // LastExecution
    }
}
