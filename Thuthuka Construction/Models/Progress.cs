using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }
        [Display(Name = "CustomerProject Id")]
        public int CustomerProjectId { get; set; }
        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject customerProject { get; set; }
        public string CurrentPhase { get; set; } 

        public DateOnly UpdateDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }

}
