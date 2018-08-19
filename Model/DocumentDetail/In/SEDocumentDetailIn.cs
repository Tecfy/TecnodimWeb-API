using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class SEDocumentDetailIn : BaseIn
    {
        [Required]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string registration { get; set; }
    }
}
