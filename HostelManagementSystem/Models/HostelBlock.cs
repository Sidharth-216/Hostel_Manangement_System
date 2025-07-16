using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HostelManagementSystem.Models
{
    public class HostelBlock
    {
        [Key]
        public int BlockId { get; set; }

        [Required]
        public string BlockName { get; set; }

        public int TotalRooms { get; set; }

        
    }
}
