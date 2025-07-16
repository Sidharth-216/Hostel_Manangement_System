namespace HostelManagementSystem.Models.ViewModels
{
    public class ApplicationFormViewModel
    {
        // Add properties as needed, for example:
        public string FullName { get; set; }
        public string Rollno { get; set; }
        public string Gender { get; set; }
        public string Branch { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public int? RoomId { get; set; } // nullable if not assigned yet

        public string RegNo { get; set; }
        public string Aadhaar { get; set; }
        public string Address { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string AlternatePhone { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherPhone { get; set; }
        public bool DeclarationAccepted { get; set; }
        public string Status { get; set; } = "Pending";
        public string StudentId { get; set; }
        // Add other properties as required by your application
    }
}