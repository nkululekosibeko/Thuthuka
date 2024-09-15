using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Thuthuka_Construction.Models
{
    public class CustomerProject
    {
        [Key]
        public int CustomerProjectId { get; set; }

        [Display(Name = "Cutomer Id")]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public ApplicationUser Customer { get; set; }


        [Display(Name = "Project Id")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [ValidateNever]
        public Project Project { get; set; }

        public DateOnly SelectDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);


        public Status Status { get; set; }
    }



        public enum Status
        {
            PendingQuotation,
            QuotationSent,
            PendingPayment,

        }

    
}