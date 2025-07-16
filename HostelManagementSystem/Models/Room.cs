using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagementSystem.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        public string Block { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int HostelBlockBlockId { get; set; }

       
    }
}
