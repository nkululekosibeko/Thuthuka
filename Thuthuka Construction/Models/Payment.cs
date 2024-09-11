using System.ComponentModel.DataAnnotations;

namespace Thuthuka_Construction.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string Status { get; set; }

        //Add foreign keys
    }
}
