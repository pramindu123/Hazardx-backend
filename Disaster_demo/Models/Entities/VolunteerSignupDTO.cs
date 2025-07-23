using System.ComponentModel.DataAnnotations;

namespace Disaster_demo.Models.Entities
{
    public class VolunteerSignupDTO
    {
        public string Name { get; set; }
        public string District { get; set; }
        public string DivisionalSecretariat { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number")]
        public string Password { get; set; }
    }
}