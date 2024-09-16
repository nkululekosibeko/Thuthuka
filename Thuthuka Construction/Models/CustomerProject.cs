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


        [Display(Name = "Quatation Id")]
        public int? QuatationId { get; set; }
        [ForeignKey("QuatationId")]
        [ValidateNever]
        public Quatation Quatation { get; set; }

        public DateOnly SelectDate { get; set; }

        public string Status { get; set; } = " pendind quatation";
    }
    
}