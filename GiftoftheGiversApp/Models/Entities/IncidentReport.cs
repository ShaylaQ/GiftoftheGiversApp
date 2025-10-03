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
        public string UserId { get; set; } // Foreign key for the Identity User
        
        public DateTime Date { get; set; }

        [Required]
        public string Incident { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

      
           }
}

