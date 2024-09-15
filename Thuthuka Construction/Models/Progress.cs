using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }

        public int CustomerProjectId { get; set; }
        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject customerProject { get; set; }
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
    }

}
