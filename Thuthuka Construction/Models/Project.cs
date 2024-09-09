namespace Thuthuka_Construction.Models
{
    public class Project
    {
        public int projectId { get; set; }
        public string name { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public string status { get; set; }

        //Add foreign keys
    }
}
