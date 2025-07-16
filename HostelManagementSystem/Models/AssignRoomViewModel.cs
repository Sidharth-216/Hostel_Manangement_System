using System.Collections.Generic;

namespace HostelManagementSystem.Models
{
    public class AssignRoomViewModel
    {
        public HostelApplication Application { get; set; }
        public List<Room> Rooms { get; set; }
        public int SelectedRoomId { get; set; }
    }
}
