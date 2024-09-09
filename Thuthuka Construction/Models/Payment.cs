namespace Thuthuka_Construction.Models
{
    public class Payment
    {
        public int paymentId { get; set; }
        public double amount { get; set; }
        public DateOnly paymentDate { get; set; }
        public string status { get; set; }

        //Add foreign keys
    }
}
