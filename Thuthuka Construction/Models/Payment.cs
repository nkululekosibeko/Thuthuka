using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thuthuka_Construction.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public DateOnly PaymentDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public ApplicationUser Customer { get; set; }

        [Display(Name = "Project Id")]
        public int CustomerProjectId { get; set; }  // New field to link the project

        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject CustomerProject { get; set; }

        public string Status { get; set; } = "Pending";

    }
}
