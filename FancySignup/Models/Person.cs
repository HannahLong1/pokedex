using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace FancySignup.Models
{
    public class Person
    {
        [Key]
        [Required]
        [EmailAddress]
        public string Email { get; set; } // Primary Key

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
