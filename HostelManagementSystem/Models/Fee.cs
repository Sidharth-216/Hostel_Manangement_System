using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HostelManagementSystem.Models
{
    public class Fees
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public HostelApplication? Application { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string PaymentStatus { get; set; } = "Unpaid";

        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }


}
