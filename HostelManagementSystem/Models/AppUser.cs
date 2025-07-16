using Microsoft.AspNetCore.Identity;

namespace HostelManagementSystem.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Branch { get; set; }
        public string? Gender { get; set; }
        public string? Rollno { get; set; }
        

        
    }
}
