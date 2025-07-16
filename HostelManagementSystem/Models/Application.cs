using System.ComponentModel.DataAnnotations;

namespace HostelManagementSystem.Models
{
public class HostelApplication
{
    public int Id { get; set; }

    public string? FullName { get; set; }
    public string? RollNo { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public string? Branch { get; set; }
    public string? PhoneNumber { get; set; }

    [Required]
    public string RegNo { get; set; }

    [Required]
    public string Aadhaar { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string FatherName { get; set; }

    [Required]
    public string MotherName { get; set; }

    [Required]
    public string AlternatePhone { get; set; }

    [Required]
    public string FatherOccupation { get; set; }

    [Required]
    public string FatherPhone { get; set; }

    [Required]
    public bool DeclarationAccepted { get; set; }
    public string Status { get; set; } = "Pending";

}
}
 