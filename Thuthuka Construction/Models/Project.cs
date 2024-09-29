using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        [Display(Name = "ProjectType Id")]

        public int ProjectTypeId { get; set; }
        [ForeignKey("ProjectTypeId")]
        [ValidateNever]
        public ProjectType ProjectType { get; set; }

        [Display(Name = "Foreman Id")]
        public string ForemanId { get; set; }
        [ForeignKey("ForemanId")]
        [ValidateNever]
        public ApplicationUser Foreman { get; set; }
    }
}
