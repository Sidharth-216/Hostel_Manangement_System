namespace HostelManagementSystem.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int AssignedRooms { get; set; }
    }
}
