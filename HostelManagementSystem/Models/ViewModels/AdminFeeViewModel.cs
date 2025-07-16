namespace HostelManagementSystem.Models.ViewModels
{
    public class AdminFeeViewModel
    {
        public string RegNo { get; set; }
        public string StudentName { get; set; }
        public string RoomNumber { get; set; }
        public decimal FeeAmount { get; set; }
        public string FeeStatus { get; set; }
        public DateTime? FeeGeneratedOn { get; set; }
    }
}
