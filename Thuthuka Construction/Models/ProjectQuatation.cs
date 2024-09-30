using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Thuthuka_Construction.Models
{
    public class ProjectQuatation
    {
    }


    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Display(Name = "Resource Name")]
        public string Name { get; set; }

        [Display(Name = "Price Per Unit")]
        public double PricePerUnit { get; set; }
    }

    public class Quotation
    {
        [Key]
        public int QuotationId { get; set; }

        [Display(Name = "Date Created")]
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Total Cost")]
        public double TotalCost { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Customer Project Id")]
        public int CustomerProjectId { get; set; }

        [ForeignKey("CustomerProjectId")]
        [ValidateNever]
        public CustomerProject CustomerProject { get; set; }

        // Navigation Property: A quotation can have multiple resources (QuotationResource)
        public ICollection<QuotationResource> QuotationResources { get; set; } // List of resources

    }

    public class QuotationResource
    {
        [Key]
        public int QuotationResourceId { get; set; }

        [Display(Name = "Quotation Id")]
        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        [ValidateNever]
        public Quotation Quotation { get; set; }

        [Display(Name = "Resource Id")]
        public int ResourceId { get; set; }

        [ForeignKey("ResourceId")]
        [ValidateNever]
        public Resource Resource { get; set; }

        public int Quantity { get; set; }
    }


}
