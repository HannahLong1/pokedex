using System.ComponentModel.DataAnnotations;

namespace FancySignup.ViewModels
{
    public class SignupViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^[A-Za-z].*(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$",
            ErrorMessage = "Password must start with a letter, include uppercase, lowercase, and a digit.(And the uppercase can't just be the first letter)")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
