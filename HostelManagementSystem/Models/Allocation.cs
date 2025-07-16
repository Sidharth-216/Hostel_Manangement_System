using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagementSystem.Models
{
    public class Allocation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Room selection is required.")]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room? Room { get; set; }  

        [Required(ErrorMessage = "ApplicationId is required.")]
        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public HostelApplication? Application { get; set; }  
    }
}
