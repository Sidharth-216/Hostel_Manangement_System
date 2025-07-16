namespace HostelManagementSystem.Models
{
    public class StudentDashboardViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RegNo { get; set; }
        public string Rollno{ get; set; }
        public string Branch { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }

        public string RoomNumber { get; set; }
        public string BlockName { get; set; }

        public decimal FeeAmount { get; set; }
        public string FeeStatus { get; set; }
        public DateTime? FeeGeneratedOn { get; set; }
    }

}

