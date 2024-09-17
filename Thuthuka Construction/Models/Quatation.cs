using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Quatation
    {
        [Key]
        public int QuatationId { get; set; }
        [Display(Name = "CustomerProject Id")]
        public int CustomerProjectId { get; set; }
        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject customerProject { get; set; }
        [Display(Name = "Foreman Id")]
        public string ForemanId { get; set; }
        [ForeignKey("ForemanId")]
        [ValidateNever]
        public ApplicationUser Foreman { get; set; }
        public Double TotalCost { get; set; }
        public string Resources { get; set; }
<<<<<<< HEAD
        public DateOnly DateCreated { get; set; }
=======
        public DateOnly SelectDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
>>>>>>> d7d0007e1aa5d3c57322c4fe9ebb4f4b85619fbe

        public string Status { get; set; } = "Sent";

        //Add foreign keys
    }
}
