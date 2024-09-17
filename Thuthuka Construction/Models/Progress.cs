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
<<<<<<< HEAD
        public string CurrentPhase { get; set; } 

        public DateOnly UpdateDate { get; set; }  
=======
        public CurrentPhase CurrentPhase { get; set; }

        public DateOnly SelectDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }

    public enum CurrentPhase
    {
        Design ,
        Excavation,
        Foundation,
        Roofing,
        Framing,
        Plumbing,
        Painting,
        Fencing,
        WindowInstallation,
>>>>>>> d7d0007e1aa5d3c57322c4fe9ebb4f4b85619fbe
    }

}
