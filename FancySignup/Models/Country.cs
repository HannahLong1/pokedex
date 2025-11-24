using System.ComponentModel.DataAnnotations;

namespace FancySignup.Models
{
    public class Country
    {
        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
