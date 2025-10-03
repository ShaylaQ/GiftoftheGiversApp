using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftoftheGiversApp.Models.Entities
{
    [Table("IncidentReports")]
    public class IncidentReport
    {
        [Key]
        public int IncidentId { get; set; }

        [Required]
        public string UserId { get; set; } 
        
        public DateTime Date { get; set; }

        [Required]
        public string Incident { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        public IdentityUser User { get; set; }
    }
}

