using System.ComponentModel.DataAnnotations;

namespace Thuthuka_Construction.Models
{
    public class ProjectType
    {
        [Key]
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }

    }
}
