using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = " available";

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
