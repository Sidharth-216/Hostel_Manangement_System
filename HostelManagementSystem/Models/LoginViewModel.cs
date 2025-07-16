using System.ComponentModel.DataAnnotations;

namespace HostelManagementSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Role { get; set; }  // Student / Admin / Warden
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
