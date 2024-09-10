using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Thuthuka_Construction.Models
{
    public class ApplicationUser : IdentityUser 
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }


    }
}
