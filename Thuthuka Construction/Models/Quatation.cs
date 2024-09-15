using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Quatation
    {
        [Key]
        public int QuatationId { get; set; }

        public int CustomerProjectId { get; set; }
        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject customerProject { get; set; }
        public string ForemanId { get; set; }
        [ForeignKey("ForemanId")]
        [ValidateNever]
        public ApplicationUser Foreman { get; set; }
        public Double TotalCost { get; set; }
        public string Resources { get; set; }
        public DateOnly SelectDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public string Status { get; set; } = "Sent";

        //Add foreign keys
    }
}
