using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftoftheGiversApp.Models.Entities
{
    [Table("Donations")]
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; } 

        public DateTime Date { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int AidQuantity { get; set; }




        public IdentityUser User { get; set; }
    }
}
