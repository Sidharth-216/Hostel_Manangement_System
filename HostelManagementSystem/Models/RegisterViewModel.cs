using System.ComponentModel.DataAnnotations;

namespace HostelManagementSystem.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        
        public string? Branch { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
         public string? Rollno { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }  // Student / Admin / Warden
    }
}
