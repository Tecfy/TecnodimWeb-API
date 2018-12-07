using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ECMDocumentDetailSaveIn : BaseIn
    {
        [Required]
        [Display(Name = "UnityCode", ResourceType = typeof(i18n.Resource))]
        public string unityCode { get; set; }

        [Required]
        [Display(Name = "CPF", ResourceType = typeof(i18n.Resource))]
        public string cpf { get; set; }

        [Required]
        [Display(Name = "RG", ResourceType = typeof(i18n.Resource))]
        public string rg { get; set; }

        [Required]
        [Display(Name = "Course", ResourceType = typeof(i18n.Resource))]
        public string course { get; set; }

        [Required]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string registration { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string name { get; set; }

        [Required]
        [Display(Name = "Status", ResourceType = typeof(i18n.Resource))]
        public string status { get; set; }

        [Required]
        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        public string unity { get; set; }
    }
}
