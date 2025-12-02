using System.ComponentModel.DataAnnotations;

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

        // Already added earlier
        public bool IsAdmin { get; set; } = false;

        // New: used for disabling / locking accounts
        public bool IsActive { get; set; } = true;
    }
}
